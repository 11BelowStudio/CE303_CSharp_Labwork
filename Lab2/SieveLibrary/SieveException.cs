using System;

namespace SieveLibrary
{
    public class SieveException: Exception
    {

        public SieveException()
        {
        }
        

        public SieveException(string message):
            base(message)
        {
        }

        public SieveException(string message, Exception inner):
            base(message, inner)
        {
        }
    }
}
