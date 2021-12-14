using System.Collections.Generic;

namespace Rhyous.Odata.Filter
{
    /// <summary>An object for storing a grouping when parsing a $filter expression string.</summary>
    public class Group
    {
        /// <summary>The open group character</summary>
        protected internal char? OpenChar;
        /// <summary>The close group character</summary>
        protected internal char? CloseChar;

        /// <summary>
        /// Empty constructor. Does not set the open and close characters. 
        /// Instead, it is assumed the close character is the same as the open character.
        /// Allows for using a separate wrap character in a nested group.
        /// </summary>
        public Group() { }
        /// <summary>
        /// Sets both the open and close characters to a specific character. 
        /// Does not allow for using a separate wrap character in a nested group.
        /// <param name="wrapChar">The character used for grouping.</param>
        /// </summary>
        public Group(char wrapChar) : this(wrapChar, wrapChar) { }

        /// <summary>
        /// Sets both the open and close characters to the input characters. 
        /// Does not allow for using a separate wrap character in a nested group.
        /// </summary>
        /// <param name="openChar">The character used for opening a group.</param>
        /// <param name="closeChar">The character used for closing a group.</param>
        public Group(char openChar, char closeChar)
        {
            OpenChar = openChar;
            CloseChar = closeChar;
        }

        /// <summary>The wrap character</summary>
        public char? WrapChar
        {
            get
            {
                if (Stack.Count > 0)
                    return Stack.Peek();
                return null;
            }
        }

        /// <summary>The character stack</summary>
        public Stack<char> Stack
        {
            get { return _Stack ?? (_Stack = new Stack<char>()); }
            set { _Stack = value; }
        } private Stack<char> _Stack;

        /// <summary>A boolean for whether the group is open or closed</summary>
        public bool IsOpen => WrapChar != null;

        /// <summary>A method to open the group.</summary>
        public virtual void Open(char openChar)
        {
            if (OpenChar != null && openChar != OpenChar)
                throw new InvalidGroupingException($"The close character was this: '{openChar}', but '{CloseChar}' was expected.");
            Stack.Push(openChar);
        }

        /// <summary>A method to close the group.</summary>
        public virtual void Close(char closeChar)
        {
            if (!IsOpen)
                throw new InvalidGroupingException($"The close character was provided: '{closeChar}', but no group was open.");
            if (CloseChar == null)
            {
                if (closeChar != WrapChar)
                    throw new InvalidGroupingException($"The close character was this: '{closeChar}', but '{CloseChar}' was expected.");
                Stack.Pop();
                return;
            }                
            if (closeChar != CloseChar)
                throw new InvalidGroupingException($"The close character was this: '{closeChar}', but '{CloseChar}' was expected.");

            Stack.Pop();
        }
    }
}