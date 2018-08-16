using Newtonsoft.Json.Linq;
using Rhyous.StringLibrary;
using System;
using System.Collections;
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
            var type = entities.First()?.GetType();
            var propInfoId = type.GetProperty(details.EntityIdProperty);
            var propInfoEntityProperty = propInfoId;
            if (!string.IsNullOrWhiteSpace(details.EntityProperty) && details.EntityProperty != details.EntityIdProperty)
                propInfoEntityProperty = type.GetProperty(details.EntityProperty);
            Type dictType = typeof(Dictionary<,>).MakeGenericType(propInfoId.PropertyType, typeof(RelatedEntityCollection));
            IDictionary dict = Activator.CreateInstance(dictType) as IDictionary;
            foreach (var entity in entities)
            {
                var id = propInfoId.GetValue(entity);
                var entityPropertyValue = id;
                if (!string.IsNullOrWhiteSpace(details.EntityProperty) && details.EntityProperty != details.EntityIdProperty)
                    entityPropertyValue = propInfoEntityProperty.GetValue(entity).ToString();
                var collection = details.ToRelatedEntityCollection(id.ToString());
                dict.Add(entityPropertyValue, collection);
            }
            foreach (var re in relatedEntities)
            {
                var value = re?.Object?.GetValue(details.EntityToRelatedEntityProperty);
                var id = value.ToString().ToType(propInfoId.PropertyType);
                (dict[id] as RelatedEntityCollection).RelatedEntities.Add(re);
            }
            return new List<RelatedEntityCollection>(dict.Values as ICollection<RelatedEntityCollection>);
        }
    }
}
