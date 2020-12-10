using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class ConnectResponse : GenericResponse
    {

        //again, wanted to make this readonly, but nooooooooooooo, micro$oft doesn't want you to serialize/deserialize readonly fields
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _id;
        public ConnectResponse(bool wasSuccess, MarketInfo info, string yourID): base(wasSuccess, info)
        {
            _id = yourID;
        }

        public ConnectResponse() : base()
        {
            _id = "";
        }

        public override string GetInfo()
        {
            return _id;
        }

        public override string ToString()
        {
            return $"ConnectResponse {{\"success\":{_success},\"marketInfo\":{_marketInfo},\"info\":\"{GetInfo()}\"}}";
        }
    }
}