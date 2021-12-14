using System.Collections.Generic;

namespace Rhyous.Odata.Filter
{

    /// <summary>An interface to define a collection of FilterConverter{TEntity} implementations that could apply to a filter.</summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICustomFilterConverterCollection<TEntity>
    {
        /// <summary>A collection of FilterConverter{TEntity} implementations that could apply to a filter.</summary>
        List<IFilterConverter<TEntity>> Converters { get; }
    }
}