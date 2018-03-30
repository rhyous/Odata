using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    /// <summary>
    /// The foreign entity has the Id of the current entity.
    /// </summary>
    /// <typeparam name="T">The type of the Entity.</typeparam>
    public class RelatedEntityOneToManySorter<T> : IRelatedEntitySorter<T>
    {
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            var type = entities.First()?.GetType();
            var propInfoId = type.GetProperty(details.EntityIdProperty);
            var propInfoEntityProperty = propInfoId;
            if (!string.IsNullOrWhiteSpace(details.EntityProperty) && details.EntityProperty != details.EntityIdProperty)
                propInfoEntityProperty = type.GetProperty(details.EntityProperty);
            foreach (var entity in entities)
            {
                var id = propInfoId.GetValue(entity).ToString();
                var entityPropertyValue = id;
                if (!string.IsNullOrWhiteSpace(details.EntityProperty) && details.EntityProperty != details.EntityIdProperty)
                    entityPropertyValue = propInfoEntityProperty.GetValue(entity).ToString();
                var collection = details.ToRelatedEntityCollection(id);
                foreach (var re in relatedEntities)
                {
                    var value = re?.Object?.GetValue(details.EntityToRelatedEntityProperty)?.ToString();
                    if (value == entityPropertyValue.ToString())
                        collection.RelatedEntities.Add(re);
                }
                list.Add(collection);
            }
            return list;
        }

    }
}
