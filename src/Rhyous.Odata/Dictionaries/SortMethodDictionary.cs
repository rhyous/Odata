using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class SortMethodDictionary<T> : Dictionary<RelatedEntity.Type, Func<IEnumerable<T>, IEnumerable<RelatedEntity>, SortDetails, List<RelatedEntityCollection>>>
    {
        public SortMethodDictionary()
        {
            Add(RelatedEntity.Type.OneToOne, OneToOneSort);
            Add(RelatedEntity.Type.OneToOneForeign, OneToOneForeignSort);
            Add(RelatedEntity.Type.OneToMany, OneToManySort);
            Add(RelatedEntity.Type.ManyToOne, ManyToOneSort);
            Add(RelatedEntity.Type.ManyToMany, ManyToManySort);
        }

        internal List<RelatedEntityCollection> OneToOneSort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (relatedEntities == null || !relatedEntities.Any() || relatedEntities.Count() > 1)
                return null;
            // ManyToOneSort is the same method, except for the requirement in the above if of only having 1 related entity.
            return ManyToOneSort(entities, relatedEntities, details); 
        }

        internal List<RelatedEntityCollection> OneToOneForeignSort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            throw new NotImplementedException();
        }

        internal List<RelatedEntityCollection> OneToManySort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            throw new NotImplementedException();
        }

        internal List<RelatedEntityCollection> ManyToOneSort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
                return null;
            var list = new List<RelatedEntityCollection>();
            foreach (var entity in entities)
            {
                var collection = new RelatedEntityCollection
                {
                    Entity = details.EntityName,
                    EntityId = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString(),
                    RelatedEntity = details.RelatedEntity,
                };
                collection.Entities.AddRange(relatedEntities.Where(re => re.Id == entity.GetType().GetProperty(details.EntityToRelatedEntityProperty).GetValue(entity).ToString()));
                list.Add(collection);
            }
            return list;
        }

        internal List<RelatedEntityCollection> ManyToManySort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            throw new NotImplementedException();
        }
    }
}