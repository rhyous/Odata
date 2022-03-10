using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl
{
    /// <summary>Allows for strings starting with certain characters to be sorted last.</summary>
    public class CertainFirstCharactersLastComparer : IComparer<string>
    {
        protected readonly HashSet<char> _SortLastChars;
        protected readonly Dictionary<char, int> _CharIntValues;

        /// <summary>The constructor.</summary>
        /// <param name="sortLastChars">The characters to sort last.</param>
        /// <remarks>Strings with different last characters are sorted in order of input array.</remarks>
        public CertainFirstCharactersLastComparer(params char[] sortLastChars)
        {
            _SortLastChars = new HashSet<char>(sortLastChars);
            int i = 0;
            _CharIntValues = (_SortLastChars == null || !_SortLastChars.Any())
                           ? new Dictionary<char, int>()
                           : _SortLastChars.ToDictionary(c => c, c => i++);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than,
        /// equal to, or greater than the other.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y, as shown in the
        ///     following table. 
        ///     -1 or less =  left is less than right
        ///     0 = left equals right
        ///     1 or greater = left is greater than right.
        /// </returns>
        public int Compare(string left, string right)
        {
            if (left is null && right is null) // Both are null
                return 0;
            if (left is null && right != null) // Left is null, right isn't.
                return -1;
            if (left != null && right == null) // Left isn't null, right is.
                return 1;
            if (left == right) // strings are equal
                return 0;

            if (!left.Any() || !right.Any()         // Either is empty
            || left[0] == right[0]                  // First characters are equal
            || (!_SortLastChars.Contains(left[0])   // Neither starts with a last character
                && !_SortLastChars.Contains(right[0])))
                return left.CompareTo(right);

            // Left has last Char but right doesn't
            if (_SortLastChars.Contains(left[0]) && !_SortLastChars.Contains(right[0]))
                return 1;
            // Right has last Char but left doesn't
            if (!_SortLastChars.Contains(left[0]) && _SortLastChars.Contains(right[0]))
                return -1;

            // They both have a last character here, but not the same one
            return _CharIntValues[left[0]].CompareTo(_CharIntValues[right[0]]);
        }
    }
}
