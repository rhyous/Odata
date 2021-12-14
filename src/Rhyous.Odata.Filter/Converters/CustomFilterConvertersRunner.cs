using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>A class to perform custom filtering.</summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public class CustomFilterConvertersRunner<TEntity> : ICustomFilterConvertersRunner<TEntity>
    {
        private readonly ICustomFilterConverterCollection<TEntity> _FilterConverterCollection;

        /// <summary>The constructor</summary>
        /// <param name="filterConverterCollection">The collection of filter converters.</param>
        public CustomFilterConvertersRunner(ICustomFilterConverterCollection<TEntity> filterConverterCollection)
        {
            _FilterConverterCollection = filterConverterCollection;
        }

        /// <inheritdoc />
        public async Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            if (filter == null)
                return filter;

            var convertedFilter = filter.Clone();
            foreach (var filterConverter in _FilterConverterCollection.Converters)
            {
                if (filterConverter.CanConvert(convertedFilter))
                {
                    convertedFilter = await filterConverter.ConvertAsync(convertedFilter);
                }
            }
            if (convertedFilter.Left != null)
                convertedFilter.Left = await ConvertAsync(convertedFilter.Left); // Recursive
            if (convertedFilter.Right != null)
                convertedFilter.Right = await ConvertAsync(convertedFilter.Right); // Recursive
            return convertedFilter;
        }        
    }
}