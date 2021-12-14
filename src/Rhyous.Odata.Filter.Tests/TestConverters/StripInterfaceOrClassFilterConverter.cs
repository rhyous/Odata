using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests
{
    public class StripInterfaceOrClassFilterConverter<TEntity> : IFilterConverter<TEntity>
    {
        /// <inheritdoc />
        public bool CanConvert(Filter<TEntity> filter)
        {
            var type = typeof(TEntity);
            return filter.IsSimpleString
                && filter.NonFilter.Count(f=> f == '.') == 1
                && (filter.NonFilter.StartsWith(type.Name)
                    || type.IsInterface && (filter.NonFilter.StartsWith(type.Name)
                                        || (type.Name.StartsWith("I") && filter.NonFilter.StartsWith(type.Name.Substring(1)))));
        }

        /// <inheritdoc />
        public Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            filter.NonFilter = filter.NonFilter.Split('.')[1];
            return Task.FromResult(filter);
        }
    }
}