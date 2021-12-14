using System.Collections.Generic;

namespace Rhyous.Odata.Filter
{
    /// <summary>A collection of FilterConverter{TEntity} implementations that could apply to a filter.</summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FilterConverterCollection<TEntity> : ICustomFilterConverterCollection<TEntity>
    {
        /// <inheritdoc />
        public List<IFilterConverter<TEntity>> Converters { get; } = new List<IFilterConverter<TEntity>>();
    }
}
