using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// An exception for an invalid method type, which means a method that doesn't exist on a type.
    /// The most commonly used method is string.Contains() but if you tried to do a Contains on a type
    /// that doesn't have a Contains() method, you might get this exception.
    /// </summary>
    public class InvalidTypeMethodException : Exception
    {
        internal const string DefaultMessage = "Method '{0}' is not valid for the requested type: {1}.";

        /// <summary>
        /// The constructor with a default message.
        /// </summary>
        /// <param name="type">The Type that is missing the method.</param>
        /// <param name="method">The name of the missing method.</param>
        public InvalidTypeMethodException(Type type, string method) : base(string.Format(DefaultMessage, type, method)) { }

        /// <summary>
        /// The constructor with a default message and an appended message.
        /// </summary>
        /// <param name="type">The Type that is missing the method.</param>
        /// <param name="method">The name of the missing method.</param>
        /// <param name="msg">A message that will be appended ot the default message.</param>
        public InvalidTypeMethodException(Type type, string method, string msg) : base($"{string.Format(DefaultMessage, type, method)} {msg}") { }
    }
}