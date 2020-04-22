using System;

namespace Rhyous.Odata.Filter
{
    [Serializable]
    internal class InvalidOdataConstantException : Exception
    {
        public InvalidOdataConstantException(string message) : base(message)
        {
        }
    }
}