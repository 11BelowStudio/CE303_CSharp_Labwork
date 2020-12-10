using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared
{
    public class Request
    {

        public const int CONNECT_REQUEST = 0;
        public const int INFO_REQUEST = 1;
        public const int TRADE_REQUEST = 2;
        public const int DISCONNECT_REQUEST = 3;

        //these are properties instead of public variables because otherwise C# won't serialize/deserialize these to/from json.
        public int code
        {
            get{ return _code;}
            set { _code = value;  }
        }

        //c# bad (with json). bottom text.
        public string body
        {
            get { return _body; }
            set { _body = value; }
        }

     
        private int _code;
        
        private string _body;

        public Request() : this(-1) { }

        public Request(int theCode) : this(theCode, ""){}

        public Request(int theCode, string theBody){
            _code = theCode;
            _body = theBody;
        }


        public int GetCode(){
            return _code;
        }

        public string GetBody(){
            return _body;
        }

        public override string ToString()
        {
            string type = "";
            switch (_code)
            {
                case CONNECT_REQUEST:
                    type = "CONNECT";
                    break;
                case INFO_REQUEST:
                    type = "INFO";
                    break;
                case TRADE_REQUEST:
                    type = "TRADE";
                    break;
                case DISCONNECT_REQUEST:
                    type = "DISCONNECT";
                    break;
                default:
                    type = "idk lol";
                    break;
            }
            return $"Request:{{ \"type\":\"{type}\",\"body\":\"{GetBody()}\"}}";
        }
    }
}