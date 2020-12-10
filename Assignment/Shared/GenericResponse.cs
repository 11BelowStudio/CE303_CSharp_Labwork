namespace Shared
{
    public class GenericResponse : IResponse
    {
        //hello yes I would like to 'talk' to the CEO of only making properties which are not read-only serializable via JSON in c#.
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        public MarketInfo marketInfo
        {
            get { return _marketInfo; }
            set {
                _marketInfo.stockholder = value.GetStockholder();
                _marketInfo.traders = value.GetTraders();
            }
        }

        public int code
        {
            get { return _code; }
            set { _code = value; }
        }
        
        internal bool _success;

        internal readonly MarketInfo _marketInfo;

        internal int _code;

        public GenericResponse(bool wasSuccess, MarketInfo info, int rCode)
        {
            _success = wasSuccess;
            _marketInfo = info;
            _code = rCode;
        }

        public GenericResponse(bool wasSuccess, MarketInfo info) : this(wasSuccess, info, -1) { }

        public GenericResponse(bool wasSuccess) : this(wasSuccess, new MarketInfo(), -1) { }

        public GenericResponse(): this(false, new MarketInfo(), -1) { }

        public bool GetSuccess()
        {
            return _success;
        }

        public LoggableMarket GetMarket()
        {
            return _marketInfo;
        }

        public virtual string GetInfo()
        {
            return "";
        }

        public override string ToString()
        {
            return $"Response {{\"success\":{_success},\"marketInfo\":{_marketInfo},\"info\":\"{GetInfo()}\"}}";
        }

    }

}