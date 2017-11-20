using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntityOneToManySorter<T> : IRelatedEntitySorter<T>
    {
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var entityRelatedIdPropInfo = relatedEntities.First()?.GetType().GetProperty(details.EntityToRelatedEntityProperty);
            foreach (var entity in entities)
            {
                var id = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString();
                var collection = new RelatedEntityCollection
                {
                    Entity = details.EntityName,
                    EntityId = id,
                    RelatedEntity = details.RelatedEntity,
                };
                collection.Entities.AddRange(relatedEntities.Where(re => entityRelatedIdPropInfo.GetValue(re)?.ToString() == id));
                list.Add(collection);
            }
            return list;
        }

    }
}
