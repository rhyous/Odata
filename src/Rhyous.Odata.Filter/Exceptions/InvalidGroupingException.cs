using System;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An expection that can be thrown when the grouping in a $filter expression is invalid.
    /// </summary>
    public class InvalidGroupingException : Exception
    {
        /// <summary>
        /// A constructor that takes in a message.
        /// </summary>
        /// <param name="msg">The message</param>
        public InvalidGroupingException(string msg) : base(msg) { }
    }
}