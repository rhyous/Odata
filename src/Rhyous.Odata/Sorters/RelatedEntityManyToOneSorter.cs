using Rhyous.StringLibrary;
using System;
using System.Collections;
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
            Type dictIdType = typeof(Dictionary<,>).MakeGenericType(propInfoId.PropertyType, typeof(RelatedEntityCollection));
            Type dictPropType = typeof(Dictionary<,>).MakeGenericType(entityRelatedIdPropInfo.PropertyType, dictIdType);
            IDictionary dict = Activator.CreateInstance(dictPropType) as IDictionary;
            var tryGetValueMethod = dictPropType.GetMethod("TryGetValue");
            var tryGetValueMethod2 = dictIdType.GetMethod("TryGetValue");
            foreach (var entity in entities)
            {
                var id = propInfoId.GetValue(entity);
                var relatedEntityId = entityRelatedIdPropInfo.GetValue(entity);
                RelatedEntityCollection collection = null;
                IDictionary idDict = null;
                object[] dictPropParameters = new object[] { relatedEntityId, null };
                if ((bool)tryGetValueMethod.Invoke(dict, dictPropParameters))
                {
                    object[] dictIdParams = new object[] { id, null };
                    if (!(bool)tryGetValueMethod2.Invoke((dictPropParameters[1] as IDictionary), dictIdParams))
                    {
                        collection = details.ToRelatedEntityCollection(id.ToString());
                        (dictPropParameters[1] as IDictionary).Add(id, collection);
                        list.Add(collection);
                    }
                    continue;
                }
                idDict = Activator.CreateInstance(dictIdType) as IDictionary;
                collection = details.ToRelatedEntityCollection(id.ToString());
                idDict.Add(id, collection);
                list.Add(collection);
                dict.Add(relatedEntityId, idDict);
            }
            foreach (var re in relatedEntities)
            {
                var id = re.Id.ToType(entityRelatedIdPropInfo.PropertyType);
                if (details.RelatedEntityIdProperty != Constants.DefaultIdProperty)
                {
                    var propValue = re?.Object?.GetValue(details.RelatedEntityIdProperty)?.ToString();
                    if (string.IsNullOrWhiteSpace(propValue))
                        propValue = re?.Object?.GetValue("Object")?[details.RelatedEntityIdProperty]?.ToString();
                    if (string.IsNullOrWhiteSpace(propValue))
                        continue;
                    id = propValue.ToType(entityRelatedIdPropInfo.PropertyType);
                }
                if (dict[id] is IDictionary idDict)
                {
                    foreach (var v in idDict.Values)
                    {
                        if (v is RelatedEntityCollection rec)
                            rec.RelatedEntities.Add(re);
                    }
                }
            }
            return list;
        }
    }
}