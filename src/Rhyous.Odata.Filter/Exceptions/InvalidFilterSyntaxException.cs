using System;

namespace Rhyous.Odata
{
    public class InvalidFilterSyntaxException : Exception
    {
        public InvalidFilterSyntaxException(int index, string syntax) : base($"The filter syntax is incorrect at index {index}: {syntax}") { }
    }
}
