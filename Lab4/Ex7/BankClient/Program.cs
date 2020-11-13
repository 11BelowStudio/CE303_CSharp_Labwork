using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BankClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your customer ID:");

            try
            {
                int customerId = int.Parse(Console.ReadLine());

                using (Client client = new Client(customerId))
                {
                    Console.WriteLine("Logged in successfully.");

                    while (true)
                    {
                        int[] accountNumbers = client.GetAccountNumbers();
                        Console.WriteLine("Your accounts:");
                        foreach (int account in accountNumbers)
                        {
                            Console.WriteLine($"  Account {account,5}: balance {client.GetBalance(account),10} GBP");
                        }


                        Console.WriteLine("Please choose an option, by entering a character into the console:");
                        Console.WriteLine("A: View your accounts");
                        Console.WriteLine("B: Transfer funds between accounts");
                        Console.WriteLine("C: Create a new account");


                        Console.WriteLine("\nEnter the appropriate character for the operation you wish to perform.");
                        char option = Console.ReadLine().ToLower()[0];

                        switch (option)
                        {
                            case 'a':
                                break;
                            case 'b':
                                Console.WriteLine("Enter the account number to transfer from:");
                                int fromAccount = int.Parse(Console.ReadLine());
                        

                                Console.WriteLine("Enter the account number to transfer to (this could be someone else's account):");
                                int toAccount = int.Parse(Console.ReadLine());

                                Console.WriteLine("Enter the amount to be transferred:");
                                int amount = int.Parse(Console.ReadLine());

                                client.Transfer(fromAccount, toAccount, amount);
                                break;
                            case 'c':
                                client.MakeAccount();
                                break;
                            default:
                                break;
                        }

                        /*
                        Console.WriteLine("Enter the account number to transfer from or -1 to print the account list:");
                        int fromAccount = int.Parse(Console.ReadLine());
                        if (fromAccount < 0)
                            continue;

                        Console.WriteLine(
                            "Enter the account number to transfer to (this could be someone else's account):");
                        int toAccount = int.Parse(Console.ReadLine());

                        Console.WriteLine("Enter the amount to be transferred:");
                        int amount = int.Parse(Console.ReadLine());

                        client.Transfer(fromAccount, toAccount, amount);

                        */
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        private static void Transfer(Client client) {
            Console.WriteLine("Enter the account number to transfer from or -1 to print the account list:");
            int fromAccount = int.Parse(Console.ReadLine());
                        

            Console.WriteLine("Enter the account number to transfer to (this could be someone else's account):");
            int toAccount = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the amount to be transferred:");
            int amount = int.Parse(Console.ReadLine());

            client.Transfer(fromAccount, toAccount, amount);
        }

        private static void MakeAccount(Client client){

        }
    }
}
