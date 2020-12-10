using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class ServerMain
    {

        private static int PORT = 8888;

        private static readonly ServerLogger logger = new ServerLogger();


        private static readonly Market theMarket = new Market(logger);
        

        public static void Main(string[] args)
        {
            
            //if there are command line arguments, attempt to set PORT to whatever number in the 1st command line argument was
            if (args.Length > 0)
            {
                try
                {
                    PORT = int.Parse(args[0]);
                } catch (InvalidCastException e)
                {
                    //and if it wasn't a number, complain (and PORT will still be 8888 in this case)
                    logger.LogError($"{args[0]} isn't an integer >:( so imma just connect to port 8888 anyway");
                }
            }

            RunTheServer();



        }


        private static void RunTheServer()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Loopback, PORT);
                listener.Start();
                logger.LogOtherMessage("Waiting for incoming connections...");
                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    new Thread(new TraderHandler(tcpClient, theMarket, logger).RunThread).Start();
                }
            } catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }
    }


}
