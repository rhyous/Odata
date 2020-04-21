using System;

namespace Rhyous.Odata.Filter.Parsers
{
    [Serializable]
    internal class InvalidOdataConstantException : Exception
    {
        public InvalidOdataConstantException(string message) : base(message)
        {
        }
    }
}