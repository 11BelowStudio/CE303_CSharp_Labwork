using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public interface IResponse
    {

        public bool GetSuccess();

        public LoggableMarket GetMarket();

        public string GetInfo();
    }
}
