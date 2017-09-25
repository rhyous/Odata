using System;

namespace Rhyous.Odata
{
    public class Group
    {
        public Group() { }
        public Group(char wrapChar) { WrapChar = wrapChar; }

        public Char? WrapChar { get; set; }
        public bool IsOpen =>WrapChar != null;

        public void Open(char wrapChar) { WrapChar = wrapChar; }
        public void Close() { WrapChar = null; }
    }
}
