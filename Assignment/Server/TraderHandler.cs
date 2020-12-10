using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.Json;
using Shared;

namespace Server
{
    public class TraderHandler
    {

        private readonly string id;

        private readonly TcpClient theClient;

        private readonly Market theMarket;

        private readonly ServerLogger logger;

        private static Barrier CLIENT_BARRIER = new Barrier(0);

        private static Object BARRIER_LOCK_OBJECT = new object();

        private static CancellationTokenSource CANCEL_SOURCE = new CancellationTokenSource();

        private static void CHANGE_CLIENT_COUNT(bool clientJoined)
        {
            lock (BARRIER_LOCK_OBJECT) {
                if (clientJoined)
                {
                    CLIENT_BARRIER.AddParticipant();
                }
                else
                {
                    CLIENT_BARRIER.RemoveParticipant();
                }
				//cancel the cancellation token that CLIENT_BARRIER observes
                CANCEL_SOURCE.Cancel();
				//and also reset it so it doesn't do anything silly
				CANCEL_SOURCE = new CancellationTokenSource();
            }
        }

        
        
        public TraderHandler(TcpClient client, Market market, ServerLogger log)
        {
            theClient = client;
            theMarket = market;
            logger = log;

            //Generates the ID for this trader via GUID (which is what microsoft calls UUIDs for some silly reason)
            id = Guid.NewGuid().ToString().ToLower();
        }

		//it's trader time
        public void RunThread()
        {
            //this client is added to the barrier
            CHANGE_CLIENT_COUNT(true);
            try
            {
                using (Stream clientStream = theClient.GetStream())
                {
                    StreamWriter writer = new StreamWriter(clientStream);
                    StreamReader reader = new StreamReader(clientStream);

                    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false,
                        PropertyNameCaseInsensitive = true
                    };

                    try
                    {
                        Request theRequest = JsonSerializer.Deserialize<Request>(reader.ReadLine(), jsonOptions);

                        bool connected = false;

                        string responseString = "";

                        if (theRequest.GetCode() == Request.CONNECT_REQUEST)
                        {
                            connected = true;
                            theMarket.AddNewTrader(this);
                            responseString = JsonSerializer.Serialize<ConnectResponse>(new ConnectResponse(true, theMarket.GetMarketInfo(), id), jsonOptions);
                        }
                        else
                        {
                            responseString = JsonSerializer.Serialize<GenericResponse>(new GenericResponse(false), jsonOptions);
                        }
                        writer.WriteLine(responseString);
                        writer.Flush();

                        

                        while (connected)
                        {
                            //true if this client has the stock, false otherwise.
                            bool hasTheStock = theMarket.GetStockholder().Equals(id);

                            bool waitForMarketUpdate = false;

                            try
                            {
                                //read the request
                                theRequest = JsonSerializer.Deserialize<Request>(reader.ReadLine(), jsonOptions);

                                switch (theRequest.code)
                                {
                                    case Request.INFO_REQUEST: //if it's an info request
                                        if (hasTheStock)
                                        {
                                            //if this is the stockholder, get the updated market info, and cancel the CLIENT_BARRIER cancel token
                                            responseString = JsonSerializer.Serialize<GenericResponse>(new GenericResponse(true, theMarket.GetMarketInfo()), jsonOptions);
                                            CANCEL_SOURCE.Cancel();
                                        }
                                        else
                                        {
                                            //if this isn't the stockholder, wait for the market to actually update before responding
                                            waitForMarketUpdate = true;
                                        }
                                        break;
                                    case Request.TRADE_REQUEST: //if it's a trade request
                                                                //go and resolve the trade request (it'll fail if it's a bad request anyway)
                                        responseString = JsonSerializer.Serialize<GiveStockResponse>(theMarket.AttemptToGiveStock(this, theRequest.body), jsonOptions);
                                        if (hasTheStock)
                                        {
                                            //if this user is (or, if the trade request was successful, was) the stockholder, cancel the CLIENT_BARRIER cancel token.
                                            CANCEL_SOURCE.Cancel();
                                        }
                                        break;
                                    case Request.DISCONNECT_REQUEST:
                                        connected = false; //if a disconnect is requested, stop the looping
                                        break;
                                    default:
                                        //if the user sent a stupid request (like another connect request, or a completely invalid request), give them a generic 'wtf you trying to do' response.
                                        responseString = JsonSerializer.Serialize<GenericResponse>(new GenericResponse(false, theMarket.GetMarketInfo()), jsonOptions);
                                        break;
                                }

                                if (waitForMarketUpdate) //if it needs to wait for a market update
                                {
                                    try
                                    {
                                        //wait at the CLIENT_BARRIER (until either everything reaches the barrier, CANCEL_SOURCE gets cancelled, or 1 minute passes)
                                        CLIENT_BARRIER.SignalAndWait(TimeSpan.FromMinutes(1), CANCEL_SOURCE.Token);
                                    }
                                    catch (OperationCanceledException e) //if the barrier gets cancelled
                                    {
                                        //reset the CANCEL_SOURCE token
                                        CANCEL_SOURCE = new CancellationTokenSource();
                                    }
                                    //response will be the current market info
                                    responseString = JsonSerializer.Serialize<GenericResponse>(new GenericResponse(true, theMarket.GetMarketInfo()), jsonOptions);

                                }
                                if (connected) //if user still connected (and expecting a response)
                                {
                                    //write the response to the output stream
                                    writer.WriteLine(responseString);
                                    writer.Flush();
                                }
                            }
                            catch (ArgumentNullException e)
                            {
                                logger.LogError(e.ToString()); //uh oh exceptiony
                            }
                            catch (JsonException j)
                            {
                                logger.LogError(j.ToString()); //jason!?!?!?!?!?
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.ToString()); //report any big problems
                    }
                    finally //once a user disconnects
                    {
                        //remove that user from the market
                        theMarket.RemoveTrader(this);

                        //give them a disconnect response (no market info because disconnected traders obviously won't need market info any more)
                        writer.WriteLine(
                            JsonSerializer.Serialize<GenericResponse>(new GenericResponse(true), jsonOptions)
                        );
                        writer.Flush();

                        //close the reader and writer streams, as well as theClient socket.
                        writer.Close();
                        reader.Close();
                        theClient.Close();

                        

 
                        
                    }
                }
            }
            catch (Exception e)
            {
                //logger.LogError($"Looks like {id} went and killed their connection. Oh well.");
            }
            //this client is removed from the barrier
            CHANGE_CLIENT_COUNT(false);
        }

		//obtains the ID of the trader connected to this TraderHandler
        public string GetId()
        {
            return id;
        }
    }
}
