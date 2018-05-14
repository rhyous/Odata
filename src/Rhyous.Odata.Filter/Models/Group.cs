using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    public class Group
    {
        public Group() { }
        public Group(char wrapChar) { Open(wrapChar); }

        public Char? WrapChar
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

        public void Open(char wrapChar) { Stack.Push(wrapChar); }
        public void Close(char wrapChar)
        {
            var c = Stack.Peek();
            if (c != WrapChar)
                throw new Exception("");
            Stack.Pop();
        }
    }
}
