using System.Collections;
using System.Collections.Generic;

namespace Rhyous.Odata.Expand.Tests
{
    public class ExpandPathComparer : IComparer, IComparer<ExpandPath>
    {
        public int Compare(object x, object y)
        {
            return Compare((ExpandPath)x, (ExpandPath)y);
        }

        public int Compare(ExpandPath x, ExpandPath y)
        {
            if (x == null || y == null)
                return 0; // if both are null they are equal
            var entityCompare = string.Compare(x.Entity, y.Entity);
            if (entityCompare != 0)
                return entityCompare;
            var paranthesisCompare = string.Compare(x.Parenthesis, y.Parenthesis);
            if (paranthesisCompare != 0)
                return paranthesisCompare;
            return Compare(x.SubExpandPath, y.SubExpandPath);
        }
    }
}