using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntityManyToOneSorter<T> : IRelatedEntitySorter<T>
    {
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var entityRelatedIdPropInfo = entities.First()?.GetType().GetProperty(details.EntityToRelatedEntityProperty);
            foreach (var entity in entities)
            {
                var collection = new RelatedEntityCollection
                {
                    Entity = details.EntityName,
                    EntityId = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString(),
                    RelatedEntity = details.RelatedEntity,
                };
                collection.RelatedEntities.AddRange(relatedEntities.Where(re => re.Id == entityRelatedIdPropInfo.GetValue(entity).ToString()));
                list.Add(collection);
            }
            return list;
        }
    }
}
