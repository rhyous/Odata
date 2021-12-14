using Rhyous.StringLibrary;

namespace Rhyous.Odata.Filter
{
    /// <summary>Extension methods for Filter{TEntity}.</summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// A method to contain an Filter{TEntity} in a new parent Filter{TEntity}.
        /// </summary>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <param name="filter">The Filter{TEntity} to contain.</param>
        /// <param name="conj">Whether to contain this with an AND or an OR operator.</param>
        /// <returns>A new parent Filter{TEntity}.</returns>
        public static Filter<TEntity> Contain<TEntity>(this Filter<TEntity> filter, Conjunction conj)
        {
            if (filter.Parent == null )
                return filter.ContainLeft(conj);

            if (conj == Conjunction.And || filter.Parent.Method.ToEnum<Conjunction>() != Conjunction.And)
                return filter.ContainLeft(conj, filter.Parent.Right = new Filter <TEntity>());

            Filter<TEntity> containerFilter = null;

            var parent = filter.Parent;
            while (parent.IsComplete && parent.Parent != null)
            {
                parent = parent.Parent;
            }
            return parent.ContainLeft(conj, containerFilter);
        }

        /// <summary>
        /// A method to containing a Filter{TEntity} in a new or provided parent Filter{TEntity}, specifically putting the filter to contain on the left.
        /// </summary>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <param name="filter">The Filter{TEntity} to contain.</param>
        /// <param name="conj">Whether to contain this with an AND or an OR operator.</param>
        /// <param name="containerFilter">Optional. The parent Filter{TEntity}. Leave blank to create a new one.</param>
        /// <returns>The provided parent Filter{TEntity} or a new one.</returns>
        public static Filter<TEntity> ContainLeft<TEntity>(this Filter<TEntity> filter, Conjunction conj, Filter<TEntity> containerFilter = null)
        {
            containerFilter = containerFilter ?? new Filter<TEntity>();
            containerFilter.Parent = filter.Parent;
            containerFilter.Method = conj.ToString();
            containerFilter.Left = filter;
            if (containerFilter.Right == null)
                containerFilter.Right = new Filter<TEntity>();
            return containerFilter;
        }

        /// <summary>
        /// A method to containing a Filter{TEntity} in a new or provided parent Filter{TEntity}, specifically putting the filter to contain on the right.
        /// </summary>
        /// <typeparam name="TEntity">The entity.</typeparam>
        /// <param name="filter">The Filter{TEntity} to contain.</param>
        /// <param name="conj">Whether to contain this with an AND or an OR operator.</param>
        /// <param name="containerFilter">Optional. The parent Filter{TEntity}. Leave blank to create a new one.</param>
        /// <returns>The provided parent Filter{TEntity} or a new one.</returns>
        public static Filter<TEntity> ContainRight<TEntity>(this Filter<TEntity> filter, Conjunction conj, Filter<TEntity> containerFilter = null)
        {
            containerFilter = containerFilter ?? new Filter<TEntity>();
            containerFilter.Parent = filter.Parent;
            containerFilter.Method = conj.ToString();
            containerFilter.Right = filter;
            if (containerFilter.Left == null)
                containerFilter.Left = new Filter<TEntity>();
            return containerFilter;
        }
    }
}