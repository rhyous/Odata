﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntityManyToManySorter<T> : IRelatedEntitySorter<T>
    {
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            throw new NotImplementedException();
            //if (entities == null || !entities.Any() || relatedEntities == null || !relatedEntities.Any())
            //    return null;
            //var list = new List<RelatedEntityCollection>();
            //var relatedEntitySingletons = relatedEntities.Where(re => re is RelatedEntityManyToMany && (re as RelatedEntityManyToMany).IsSingleton);
            //var relatedEntityList = relatedEntities.Where(re => re is RelatedEntityManyToMany && !(re as RelatedEntityManyToMany).IsSingleton);
            //var entityRelatedIdPropInfo = entities.First()?.GetType().GetProperty(details.EntityToRelatedEntityProperty);
            //foreach (var relatedEntity in relatedEntitySingletons)
            //{
            //    var collection = new RelatedEntityCollection
            //    {
            //        Entity = details.EntityName,
            //        EntityId = relatedEntity.GetType().GetProperty(details.EntityIdProperty).GetValue(relatedEntity).ToString(),
            //        RelatedEntity = details.RelatedEntity,
            //    };
            //    collection.Entities.AddRange(relatedEntities.Where(re => re.Id == entityRelatedIdPropInfo.GetValue(relatedEntity).ToString()));
            //}
            //foreach (var entity in entities)
            //{
            //    var collection = new RelatedEntityCollection
            //    {
            //        Entity = details.EntityName,
            //        EntityId = entity.GetType().GetProperty(details.EntityIdProperty).GetValue(entity).ToString(),
            //        RelatedEntity = details.RelatedEntity,
            //    };
            //    collection.Entities.AddRange(relatedEntities.Where(re => re.Id == entityRelatedIdPropInfo.GetValue(entity).ToString()));
            //    list.Add(collection);
            //}
            //return list;
        }
    }
}