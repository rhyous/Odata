namespace Rhyous.Odata
{
    /// <summary>
    /// This overloads Filter{TEntity} only it is specific to when the data is an array.
    /// When a string query comes in like this:
    ///     Id in (1,2,3)
    /// the right part of the Filter should be an ArrayFilter{TEntity}.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TArrayItem">The item type of the array, which should match the type of the property of TEntity that is in the query.</typeparam>
    /// <remarks>This only works as a Filter{TEntity}.Right, so far.</remarks>
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
            if (Array != null)
            {
                return $"({string.Join(",", Array)})";
            }
            return $"{Left} {Method} {Right}";
        }
        #endregion

        /// <summary>An implicit cast from an array to an ArrayFilter{TEntity}.</summary>
        public static implicit operator ArrayFilter<TEntity, TArrayItem>(TArrayItem[] array)             
        {
            if (array is null)
                return null;
            return new ArrayFilter<TEntity, TArrayItem> { Array = array };
        }
    }
}