using System.Collections.Generic;

namespace Shared
{
    public class MarketInfo: LoggableMarket
    {
        public MarketInfo() : base() { }

        public MarketInfo(MarketInfo info) : base(info.GetTraders(), info.GetStockholder()) { }

        public MarketInfo(HashSet<string> traders, string stockholder) : base(traders, stockholder) { }

        public MarketInfo(LoggableMarket m) : base(m.GetTraders(), m.GetStockholder()) { }

        public void UpdateTraders(HashSet<string> newTraders)
        {
            traders.Clear();
            foreach(string s in newTraders)
            {
                traders.Add(s);
            }
        }

        public void UpdateStockholder(string newStockholder)
        {
            stockholder = newStockholder;
        }

        public void UpdateMarketInfo(HashSet<string> newTraders, string newStockholder)
        {
            traders.Clear();
            foreach (string s in newTraders)
            {
                traders.Add(s);
            }
            stockholder = newStockholder;
        }
        
        public MarketInfo CopyThis()
        {
            return new MarketInfo(traders, stockholder);
        }
    }
}