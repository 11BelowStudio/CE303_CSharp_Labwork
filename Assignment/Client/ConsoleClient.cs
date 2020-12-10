using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace Client
{
    public class ConsoleClient
    {
        private readonly ClientConnection theConnection;

        private readonly string id;

        private bool youHaveTheStock;

        private bool connected;

        private LoggableMarket marketInfo;

        enum REQUEST_TYPE_ENUM{
            SEND_INFO,
            SEND_TRADE,
            SEND_QUIT
        }



        private ConsoleClient(string hostname, int port)
        {
            theConnection = new ClientConnection(hostname, port);

            Console.WriteLine("Sending connect request");

            ConnectResponse connResponse = theConnection.Connect();

            if (connResponse.success)
            {
                connected = true;
                id = connResponse.id;
                marketInfo = connResponse.marketInfo;
                Console.WriteLine("Connected successfully.");
            }
            else
            {
                Console.WriteLine("Connection unsuccessful.");
                Console.WriteLine("Aborting");
                throw new Exception();
            }
        }


        private void RunTheClient()
        {
            IResponse theResponse = new GenericResponse(false);
            REQUEST_TYPE_ENUM sendThis;

            while (connected)
            {
                OutputClientInfo();
                Console.WriteLine("\nPlease select an option:");
                if (youHaveTheStock)
                {
                    sendThis = StockholderOptions();
                }
                else
                {
                    sendThis = StocklessOptions();
                }

                switch (sendThis)
                {
                    case REQUEST_TYPE_ENUM.SEND_INFO:
                        Console.WriteLine("Requesting updated market info. Please wait for the market to update.");
                        theResponse = theConnection.GetInfo();
                        break;
                    case REQUEST_TYPE_ENUM.SEND_TRADE:
                        Console.WriteLine("Please enter the ID of the trader you want to give the stock to.");
                        string sendTo = Console.ReadLine();
                        Console.WriteLine($"Sending stock to {sendTo}");
                        theResponse = theConnection.GiveStock(sendTo);
                        break;
                    case REQUEST_TYPE_ENUM.SEND_QUIT:
                        Console.WriteLine("bye");
                        connected = false;
                        break;

                }

                if (connected)
                {
                    marketInfo = theResponse.GetMarket();
                    Console.WriteLine(theResponse.GetInfo());
                }
            }
            theConnection.Dispose();
            Console.WriteLine("That's all, folks!");
        }


        private REQUEST_TYPE_ENUM StockholderOptions()
        {
            Console.WriteLine("a: Request updated market info");
            Console.WriteLine("b: Give the stock to another trader");
            Console.WriteLine("c: Disconnect");
            Console.WriteLine("\nPlease enter the letter corresponding to your chosen operation:");
            char option = 'k';
            bool notChosen = true;
            do
            {
                try
                {
                    //ensure user gives a valid input
                    option = Console.ReadLine().ToLower()[0];
                    notChosen = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please ensure you've entered a letter.");
                }
            } while (notChosen);

            switch (option)
            {
                case 'a':
                    return REQUEST_TYPE_ENUM.SEND_INFO;
                case 'b':
                    return REQUEST_TYPE_ENUM.SEND_TRADE;
                case 'c':
                    return REQUEST_TYPE_ENUM.SEND_QUIT;
                default:
                    Console.WriteLine("idk what you're trying to do, so imma just request updated market info.");
                    return REQUEST_TYPE_ENUM.SEND_INFO;
            }
        }

        private REQUEST_TYPE_ENUM StocklessOptions()
        {
            while (true)
            {
                Console.WriteLine("a: Wait for market info to update");
                Console.WriteLine("b: Disconnect");
                Console.WriteLine("\nPlease enter the letter corresponding to your chosen operation:");
                char option = 'k';
                bool notChosen = true;
                do
                {
                    try
                    {
                        //ensure user gives a valid input
                        option = Console.ReadLine().ToLower()[0];
                        notChosen = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Please ensure you've entered a letter.");
                    }
                } while (notChosen);

                switch (option)
                {
                    case 'a':
                        return REQUEST_TYPE_ENUM.SEND_INFO;
                    case 'b':
                        return REQUEST_TYPE_ENUM.SEND_QUIT;
                    default:
                        Console.WriteLine("idk what you're trying to do here. please only enter 'a' or 'b'");
                        break;
                }
            }
        }

        private void OutputClientInfo()
        {
            youHaveTheStock = marketInfo.GetStockholder().Equals(id);
            Console.WriteLine($"You are trader {id}");
            Console.WriteLine($"Market info:\n{marketInfo}");
            Console.WriteLine($"\nYou {(youHaveTheStock ? "" : "don't ")}own the stock.");
        }

        public static void Main(string[] args)
        {
            string hostname = "localhost";
            int port = 8888;
            //if there are command line arguments, attempt to set hostname to whatever the 1st command line argument was
            // and set port to whatever number the 2nd command line argument was
            if (args.Length > 0)
            {
                hostname = args[0];
                if (args.Length > 1)
                {
                    try
                    {
                        port = int.Parse(args[0]);
                    }
                    catch (InvalidCastException e)
                    {
                        //and if it wasn't a number, complain (and port will still be 8888 in this case)
                        Console.WriteLine($"{args[0]} isn't an integer >:( so imma just connect to port 8888 anyway");
                    }
                }
            }

            new ConsoleClient(hostname, port).RunTheClient();

        }
    }

    
}
