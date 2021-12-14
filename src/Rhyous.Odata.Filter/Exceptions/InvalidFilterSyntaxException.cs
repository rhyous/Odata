using System;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An exception for an invalid $filter expression syntax
    /// </summary>
    public class InvalidFilterSyntaxException : Exception
    {
        internal const string DefaultMessage = "The filter syntax is incorrect at index {0}: {1}.";

        /// <summary> 
        /// The constructor with a default message.
        /// </summary>
        /// <param name="index">The character index where the exception was thrown.</param>
        /// <param name="syntax">The syntax.</param>
        public InvalidFilterSyntaxException(int index, string syntax) : base(string.Format(DefaultMessage, index, syntax)) { }

        /// <summary>
        /// The constructor with a default message and an appended message.
        /// </summary>
        /// <param name="index">The character index where the exception was thrown.</param>
        /// <param name="syntax">The syntax.</param>
        /// <param name="msg">A message that will be appended ot the default message.</param>
        public InvalidFilterSyntaxException(int index, string syntax, string msg) : base($"{string.Format(DefaultMessage, index, syntax)} {msg}") { }
    }
}