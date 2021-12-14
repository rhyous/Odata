using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An interface for Filter converters.
    /// </summary>
    /// <typeparam name="TEntity">The entity model class.</typeparam>
    public interface IFilterConverter<TEntity>
    {
        /// <summary>
        /// A method that determines whether this IFilterConverter{TEntity} can convert
        /// the provided Filter{TEntity}.
        /// </summary>
        /// <param name="filter">The filter to convert.</param>
        /// <returns>True if this converter can convert this filter, false otherwise.</returns>
        bool CanConvert(Filter<TEntity> filter);
        /// <summary>
        /// The convert method, which converts a Filter{TEntity} either by modifying it
        /// and returning it, or be creating a new Filter{TEntity}. 
        /// </summary>
        /// <param name="filter">The filter to convert.</param>
        /// <returns>The same or new filter after being converted.</returns>
        Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter);
    }
}