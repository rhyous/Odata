using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An interface to run custom filter converters.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public interface ICustomFilterConvertersRunner<TEntity>
    {
        /// <summary>
        /// Runs the Filter{TEntity} and any of its children through all converters.
        /// </summary>
        /// <param name="filter">The Filter{TEntity} to run through all the converters.</param>
        /// <returns>A converted Filter{TEntity}, which could be the same or different instance.</returns>
        Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter);
    }
}
