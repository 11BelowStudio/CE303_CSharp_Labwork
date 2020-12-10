using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public abstract class LoggableMarket
    {

        //whose idea was it to prevent C# from being able to serialize/deserialize readonly fields to/from JSON? like seriously wtf?
        public HashSet<string> traders
        {
            get; set;
            /*
            get { return traders; }
            set {
                traders = new HashSet<string>();
                foreach (string s in value)
                {
                   traders.Add(s);
                }
            }
            */
        }

        //yes I know this is gross to look at. But noooooooo, C# just can't support serialization of readonly fields, because fuck you that's why.
        public string stockholder
        {
            get; set;
            /*
            get { return stockholder; }
            set { stockholder = value; }
            */
        }


        //internal readonly HashSet<string> traders;

        //internal string stockholder;


        public LoggableMarket(){
            traders = new HashSet<string>();
            stockholder = "";
        }

        public LoggableMarket(HashSet<string> t, string s){
            traders = new HashSet<string>();
            foreach(string x in t){
                traders.Add(x);
            }
            stockholder = s;
        }


        public HashSet<string> GetTraders()
        {
            return traders;
        }

        public string GetStockholder()
        {
            return stockholder;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (traders.Count == 0)
            {
                sb.Append("No traders in market!\n");
                sb.Append("The server holds the stock.");
            }
            else
            {
                sb.Append($"There {(traders.Count == 1 ? "is" : "are")} {traders.Count} connected trader{(traders.Count != 1 ? "s" : "")}.\n");
                foreach(string s in traders)
                {
                    sb.Append($"{s}{(s.Equals(stockholder) ? " owns the stock" : "")}\n");
                }
                sb.Append($"\nStockholder: {stockholder}");
            }
            return sb.ToString();
        }

    }
}