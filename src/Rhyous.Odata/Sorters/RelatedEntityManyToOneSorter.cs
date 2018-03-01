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
            var entityRelatedIdPropInfo = entities.First()?.GetType().GetProperty(details.EntityToRelatedEntityProperty);
            foreach (var entity in entities)
            {
                var collection = new RelatedEntityCollection
                {
                    Entity = details.EntityName,
                    EntityId = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString(),
                    RelatedEntity = details.RelatedEntity,
                };
                if (details.RelatedEntityIdProperty != SortDetails.DefaultIdProperty)
                {
                    foreach (var re in relatedEntities)
                    {
                        string json = re?.Object?.ToString();
                        if (string.IsNullOrWhiteSpace(json))
                            continue;
                        var jobject = JObject.Parse(re.Object.ToString());
                        // Look directly
                        var value = jobject.SelectToken($"{details.RelatedEntityIdProperty}")?.ToString(); 
                        if (string.IsNullOrWhiteSpace(value)) // look as if Object is json of a RelatedEntity                            
                            value = jobject.SelectToken($"Object.{details.RelatedEntityIdProperty}")?.ToString(); 
                        if (string.IsNullOrWhiteSpace(value))
                            continue;                        
                        if (entityRelatedIdPropInfo.GetValue(entity).ToString() == value)
                            collection.RelatedEntities.Add(re);
                    }
                }
                else
                {
                    collection.RelatedEntities.AddRange(relatedEntities.Where(re => re.Id == entityRelatedIdPropInfo.GetValue(entity).ToString()));
                }
                list.Add(collection);
            }
            return list;
        }
    }
}
