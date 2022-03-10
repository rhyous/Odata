using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>A comparer that puts most characters in order, except in the first position of a string, 
    /// the '@' character is considered last.</summary>
    public class @LastComparer : CertainFirstCharactersLastComparer
    {
        #region Singleton

        private static readonly Lazy<@LastComparer> Lazy = new Lazy<@LastComparer>(() => new @LastComparer());

        /// <summary>This singleton instance</summary>
        public static @LastComparer Instance { get { return Lazy.Value; } }

        internal @LastComparer() : base('@') { }

        #endregion
    }
}