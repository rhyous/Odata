using System;

namespace Rhyous.Odata
{
    public class InvalidTypeMethodException : Exception
    {
        public InvalidTypeMethodException(Type type, string method) : base($"Method '{method}' is not valid for the requested type: {type}.") { }
        public InvalidTypeMethodException(Type type, string method, string msg) : base($"Method '{method}' is not valid for the requested type: {type}. {msg}") { }
    }
}