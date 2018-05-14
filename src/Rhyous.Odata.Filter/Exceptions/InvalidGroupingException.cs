using System;

namespace Rhyous.Odata
{
    public class InvalidGroupingException : Exception
    {
        public InvalidGroupingException(string msg) : base(msg) { }
    }
}