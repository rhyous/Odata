using Rhyous.StringLibrary;
using System;
using System.Linq;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// This overloads Filter{TEntity} only it is specific to when the data is an array.
    /// When a string query comes in like this:
    ///     Id in (1,2,3)
    /// the right part of the Filter should be an ArrayFilter{TEntity}.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TArrayItem">The item type of the array, which should match the type of the property of TEntity that is in the query.</typeparam>
    /// <remarks>This only works as a Filter{TEntity}.Right, so far. Not property is completly ignored.</remarks>
    public class ArrayFilter<TEntity, TArrayItem> : Filter<TEntity>
    {
        /// <summary>The array.</summary>
        public TArrayItem[] Array { get; set; }
        /// <summary>Whether this is an Array or not. This always returns true.</summary>
        public override bool IsArray => true;

        #region ToString
        /// <summary>Overrides ToString so that it returns the array expression as a string.</summary>
        /// <returns>The array expression as a string.</returns>
        /// <example>(1,2,3,4,5)</example>
        public override string ToString()
        {
            if (Array == null)
                return string.Empty;

            TArrayItem[] tmpArray = Array;
            if (typeof(TArrayItem) == typeof(string))
            {
                var stringArray = new string[Array.Length];
                tmpArray = Array.Select(s => (s as string).EscapeAndQuoteIfNeeded().To<TArrayItem>())
                                .ToArray();
            }
            return $"({string.Join(",", tmpArray)})";
        }
        #endregion

        /// <summary>An implicit cast from an array to an ArrayFilter{TEntity}.</summary>
        public static implicit operator ArrayFilter<TEntity, TArrayItem>(TArrayItem[] array)
        {
            if (array is null)
                return null;
            return new ArrayFilter<TEntity, TArrayItem> { Array = array };
        }

        /// <summary>
        /// Clones the current $Filter{TEntity} into a new instance.
        /// </summary>
        /// <returns>A new Filter{TEntity} cloned from the original.</returns>
        public override Filter<TEntity> Clone()
        {
            return Clone(false);
        }

        /// <summary>
        /// Clones the current $Filter{TEntity} into a new instance.
        /// </summary>
        /// <param name="cloneArray">Whether to clone the Array object. It is a shallow array clone.</param>
        /// <returns>A new Filter{TEntity} cloned from the original.</returns>
        public ArrayFilter<TEntity, TArrayItem> Clone(bool cloneArray)
        {
            return new ArrayFilter<TEntity, TArrayItem> { Array = cloneArray ? Array.Clone() as TArrayItem[] : Array };
        }
    }
}