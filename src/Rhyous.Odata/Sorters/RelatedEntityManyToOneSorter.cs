using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    /// <summary>
    /// The current entity has the Id of the foreign entity.
    /// </summary>
    /// <typeparam name="T">The type of the Entity.</typeparam>
    public class RelatedEntityManyToOneSorter<T> : IRelatedEntitySorter<T>
    {
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var propInfoId = entities.First().GetType().GetProperty(details.EntityIdProperty);
            var entityRelatedIdPropInfo = entities.First()?.GetType().GetProperty(details.EntityToRelatedEntityProperty);
            foreach (var entity in entities)
            {
                var id = propInfoId.GetValue(entity).ToString();
                var collection = details.ToRelatedEntityCollection(id);
                if (details.RelatedEntityIdProperty == Constants.DefaultIdProperty)
                {
                    var relatedEntityId = entityRelatedIdPropInfo.GetValue(entity).ToString();
                    collection.RelatedEntities.AddRange(relatedEntities.Where(re => re.Id == relatedEntityId));
                    list.Add(collection);
                    continue;
                }
                foreach (var re in relatedEntities)
                {
                    var value = re?.Object?.GetValue(details.RelatedEntityIdProperty)?.ToString();
                    if (string.IsNullOrWhiteSpace(value))
                        value = re?.Object?.GetValue("Object")?[details.RelatedEntityIdProperty]?.ToString();
                    if (string.IsNullOrWhiteSpace(value))
                        continue;
                    if (entityRelatedIdPropInfo.GetValue(entity).ToString() == value)
                        collection.RelatedEntities.Add(re);
                }
                list.Add(collection);
            }
            return list;
        }
    }
}