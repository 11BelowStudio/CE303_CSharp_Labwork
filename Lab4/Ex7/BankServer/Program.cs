using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace BankServer
{
    class Program
    {
        private const int port = 8888;

        private static readonly Bank bank = new Bank();

        static void Main(string[] args)
        {
            bank.CreateAccount(1, 1001, 100);
            bank.CreateAccount(1, 1002, 200);
            bank.CreateAccount(2, 1003, 150);
            bank.CreateAccount(3, 1004, 50);

            RunServer();
        }

        private static void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.WriteLine("Waiting for incoming connections...");
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                new Thread(HandleIncomingConnection).Start(tcpClient);
            }
        }

        private static void HandleIncomingConnection(object param)
        {
            TcpClient tcpClient = (TcpClient) param;
            using (Stream stream = tcpClient.GetStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);
                int customerId = 0;
                try
                {
                    customerId = int.Parse(reader.ReadLine());
                    Console.WriteLine($"New connection; customer ID {customerId}");
                    if (bank.GetListOfAccounts(customerId).Length == 0)
                        throw new Exception($"Unknown customer: {customerId}.");
                    writer.WriteLine("SUCCESS");
                    writer.Flush();

                    while (true)
                    {
                        string line = reader.ReadLine();
                        string[] substrings = line.Split(' ');
                        switch (substrings[0].ToLower())
                        {
                            case "accounts":
                                int[] listOfAccounts = bank.GetListOfAccounts(customerId);
                                writer.WriteLine(listOfAccounts.Length);
                                foreach (int accountNumber in listOfAccounts)
                                    writer.WriteLine(accountNumber);
                                writer.Flush();
                                break;

                            case "balance":
                                int account = int.Parse(substrings[1]);
                                writer.WriteLine(bank.GetAccountBalance(customerId, account));
                                writer.Flush();
                                break;

                            case "transfer":
                                int fromAccount = int.Parse(substrings[1]);
                                int toAccount = int.Parse(substrings[2]);
                                int amount = int.Parse(substrings[3]);
                                bank.Transfer(customerId, fromAccount, toAccount, amount);
                                writer.WriteLine("SUCCESS");
                                writer.Flush();
                                break;

                            case "make_account":
                                bank.CustomerCreateAccount(customerId);
                                writer.WriteLine("SUCCESS");
                                writer.Flush();
                                break;
                            default:
                                throw new Exception($"Unknown command: {substrings[0]}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        writer.WriteLine("ERROR " + e.Message);
                        writer.Flush();
                        tcpClient.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Failed to send error message.");
                    }
                }
                finally
                {
                    Console.WriteLine($"Customer {customerId} disconnected.");
                }
            }
        }
    }
}

