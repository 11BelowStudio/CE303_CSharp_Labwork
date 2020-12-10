using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace Server
{
    public class ServerLogger
    {

        public ServerLogger() { }

        public void LogConnect(string connectedTrader)
        {
            Console.WriteLine($"Trader {connectedTrader} connected.");
        }

        public void LogDisconnect(string leftTrader)
        {
            Console.WriteLine($"Trader {leftTrader} disconnected.");
        }

        public void LogServerGaveStock(string newStockholder)
        {
            Console.WriteLine($"Server gave stock to {newStockholder}");
        }

        public void LogTrade(string sender, string recipient)
        {
            Console.WriteLine($"{sender} gave stock to {recipient}");
        }

        public void LogMarketInfo(LoggableMarket market)
        {
            Console.WriteLine($"\nMarket Info:\n{market}\n");
        }

        public void LogError(string errorMessage)
        {
            Console.WriteLine($"ERROR: {errorMessage}");
        }

        public void LogOtherMessage(string message)
        {
            Console.WriteLine(message);
        }

    }
}
