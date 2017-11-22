using Newtonsoft.Json.Linq;
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
            foreach (var entity in entities)
            {
                var id = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString();
                var collection = new RelatedEntityCollection
                {
                    Entity = details.EntityName,
                    EntityId = id,
                    RelatedEntity = details.RelatedEntity,
                };
                foreach (var re in relatedEntities)
                {
                    var jsonObj = JObject.Parse(re.Object.ToString());
                    var value = jsonObj[details.EntityToRelatedEntityProperty].ToString();
                    if (value == id.ToString())
                        collection.RelatedEntities.Add(re);
                }
                list.Add(collection);
            }
            return list;
        }

    }
}
