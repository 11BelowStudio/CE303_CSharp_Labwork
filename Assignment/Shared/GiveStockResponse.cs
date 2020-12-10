using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class GiveStockResponse : GenericResponse
    {
        public const int NOT_STOCK_OWNER = 0;
        public const int RECIPIENT_DOESNT_EXIST = 1;
        public const int GAVE_STOCK = 2;
        public const int WOW_GREEDY = 3;

        public GiveStockResponse(bool wasSuccess, MarketInfo info, int rCode) : base(wasSuccess, info, rCode) { }

        public GiveStockResponse() : base() { }

        public override string GetInfo()
        {
            switch (code)
            {
                case NOT_STOCK_OWNER:
                    return "You can't trade stocks that you don't own.\n";
                case RECIPIENT_DOESNT_EXIST:
                    return "You can't give stocks to traders that don't exist.\n";
                case GAVE_STOCK:
                    return "Successfully gave stock!";
                case WOW_GREEDY:
                    return "Congrats you gave your stock to yourself. kinda greedy ngl.\n";
                default:
                    return "idk what this response means either\n";
            }
        }

        public override string ToString()
        {
            return $"GiveStockResponse {{\"success\":{success},\"marketInfo\":{marketInfo},\"info\":\"{GetInfo()}\"}}";
        }
    }

    
}
