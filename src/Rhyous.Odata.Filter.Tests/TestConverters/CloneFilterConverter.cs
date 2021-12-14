using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests
{
    public class CloneFilterConverter<TEntity> : IFilterConverter<TEntity>
    {
        /// <inheritdoc />
        public bool CanConvert(Filter<TEntity> filter)
        {
            return true;
        }

        /// <inheritdoc />
        public Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            return Task.FromResult(filter.Clone());
        }
    }
}