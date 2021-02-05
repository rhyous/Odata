using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    public class Group
    {
        protected internal char? _OpenChar;
        protected internal char? _CloseChar;
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
            _OpenChar = openChar;
            _CloseChar = closeChar;
        }

        public char? WrapChar
        {
            get
            {
                if (Stack.Count > 0)
                    return Stack.Peek();
                return null;
            }
        }

        public Stack<char> Stack
        {
            get { return _Stack ?? (_Stack = new Stack<char>()); }
            set { _Stack = value; }
        } private Stack<char> _Stack;
        
        public bool IsOpen => WrapChar != null;

        public virtual void Open(char openChar)
        {
            if (_OpenChar != null && openChar != _OpenChar)
                throw new InvalidGroupingException($"The close character was this: '{openChar}', but '{_CloseChar}' was expected.");
            Stack.Push(openChar);
        }

        public virtual void Close(char closeChar)
        {
            if (!IsOpen)
                throw new InvalidGroupingException($"The close character was provided: '{closeChar}', but no group was open.");
            if (_CloseChar == null)
            {
                if (closeChar != WrapChar)
                    throw new InvalidGroupingException($"The close character was this: '{closeChar}', but '{_CloseChar}' was expected.");
                Stack.Pop();
                return;
            }                
            if (closeChar != _CloseChar)
                throw new InvalidGroupingException($"The close character was this: '{closeChar}', but '{_CloseChar}' was expected.");

            Stack.Pop();
        }
    }
}