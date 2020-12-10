using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace Server
{
    public class Market
    {
        private readonly Dictionary<String, TraderHandler> traders;

        private readonly MarketInfo theMarket;

        private readonly ServerLogger logger;

        private string stockholder;


        public Market(ServerLogger log)
        {
            logger = log;
            stockholder = "unknown";

            traders = new Dictionary<string, TraderHandler>();

            theMarket = new MarketInfo(new HashSet<string>(), stockholder);
        }



        internal void AddNewTrader(TraderHandler newTrader)
        {
            lock (traders)
            {
                lock (theMarket)
                {
                    logger.LogConnect(newTrader.GetId());
                    traders.Add(newTrader.GetId(), newTrader);
                    CheckRemainingTraders();
                    UpdateMarketInfo();
                }
            }
        }

        internal void RemoveTrader(TraderHandler yeetThisTrader)
        {
            lock (traders)
            {
                lock (theMarket)
                {
                    //log that the trader has disconnected
                    logger.LogDisconnect(yeetThisTrader.GetId());
                    traders.Remove(yeetThisTrader.GetId());
                    CheckRemainingTraders();
                    UpdateMarketInfo();
                }
            }
        }

        private void CheckRemainingTraders()
        {
            if (!traders.ContainsKey(stockholder))
            {
                if (traders.Count > 0)
                {
                    IEnumerator<string> traderEnumator = traders.Keys.GetEnumerator(); //get the enumerator of trader keys
                    traderEnumator.MoveNext(); //move to 1st trader
                    stockholder = traderEnumator.Current; //stockholder is the 1st trader.
                    logger.LogServerGaveStock(stockholder);
                }
            }
        }

        private void UpdateMarketInfo()
        {
            HashSet<string> allTraders = new HashSet<string>();
            foreach(string s in traders.Keys)
            {
                allTraders.Add(s);
            }
            theMarket.UpdateMarketInfo(allTraders, stockholder);
            logger.LogMarketInfo(theMarket);
        }

        internal GiveStockResponse AttemptToGiveStock(TraderHandler sender, string recipient)
        {
            GiveStockResponse response;
            string sendID = sender.GetId();
            lock (traders)
            {
                lock (theMarket)
                {
                    if (sendID.Equals(stockholder))
                    {
                        if (traders.ContainsKey(recipient))
                        {
                            if (sendID.Equals(recipient))
                            {
                                //if sender sent it to themselves, don't bother updating anything because they were being greedy.
                                response = new GiveStockResponse(true, theMarket.CopyThis(), GiveStockResponse.WOW_GREEDY);
                                logger.LogTrade(sendID, recipient);
                            }
                            else
                            {
                                //if sender sent it to somebody else, it's time to actually update the state of the market
                                stockholder = recipient;
                                theMarket.UpdateStockholder(stockholder);
                                //also let the user know that they successfully managed to trade their item
                                response = new GiveStockResponse(true, theMarket.CopyThis(), GiveStockResponse.GAVE_STOCK);
                                logger.LogTrade(sendID, recipient);
                                logger.LogMarketInfo(theMarket);

                            }
                        }
                        else
                        {
                            //cant give stocks to people what don't exist
                            response = new GiveStockResponse(false, theMarket.CopyThis(), GiveStockResponse.RECIPIENT_DOESNT_EXIST);
                        }
                    }
                    else
                    {
                        //can't send stonks you don't own
                        response = new GiveStockResponse(false, theMarket.CopyThis(), GiveStockResponse.NOT_STOCK_OWNER);
                    }
                }
            }
            return response;
        }

        internal MarketInfo GetMarketInfo()
        {
            lock (theMarket)
            {
                return theMarket.CopyThis();
            }
        }

        public string GetStockholder()
        {
            return stockholder;
        }


    }
}
