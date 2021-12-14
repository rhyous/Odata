using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests
{
    public class IdToEntityIdFilterConverter<TEntity> : IFilterConverter<TEntity>
    {
        /// <inheritdoc />
        public bool CanConvert(Filter<TEntity> filter)
        {
            return filter != null && filter.IsSimpleString && filter.NonFilter == "Id";
        }

        /// <inheritdoc />
        public async Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            await Task.CompletedTask; // Avoids dumb warnings when nothing inside this method is async
            if (!CanConvert(filter))
                return filter;
            var entityType = typeof(TEntity);
            var name = entityType.Name;
            if (entityType.IsInterface && entityType.Name.StartsWith("I"))
                name = name.Substring(1);
            filter.NonFilter = $"{name}Id";
            return filter;
        }
    }
}