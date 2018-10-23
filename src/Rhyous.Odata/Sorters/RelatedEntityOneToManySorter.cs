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
            var list = new List<RelatedEntityCollection>();
            var type = entities.First()?.GetType();
            var propInfoId = type.GetProperty(details.EntityIdProperty);
            var entityPropertyPropInfo = type.GetProperty(details.EntityProperty);
            
            Type dictIdType = typeof(Dictionary<,>).MakeGenericType(propInfoId.PropertyType, typeof(RelatedEntityCollection));
            Type dictPropType = typeof(Dictionary<,>).MakeGenericType(entityPropertyPropInfo.PropertyType, dictIdType);
            Type dictType = typeof(Dictionary<,>).MakeGenericType(propInfoId.PropertyType, typeof(RelatedEntityCollection));
            IDictionary dict = Activator.CreateInstance(dictPropType) as IDictionary;
            var tryGetValueMethod = dictPropType.GetMethod("TryGetValue");
            var tryGetValueMethod2 = dictIdType.GetMethod("TryGetValue");
            foreach (var entity in entities)
            {
                var id = propInfoId.GetValue(entity);
                var entityPropertyValue = entityPropertyPropInfo.GetValue(entity);
                RelatedEntityCollection collection = null;
                IDictionary idDict = null;
                object[] dictPropParameters = new object[] { entityPropertyValue, null };
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
                dict.Add(entityPropertyValue, idDict);
            }
            foreach (var re in relatedEntities)
            {
                var value = re?.Object?.GetValue(details.EntityToRelatedEntityProperty)?.ToString();
                var typedValue = value.ToType(entityPropertyPropInfo.PropertyType);                
                if (dict[typedValue] is IDictionary idDict)
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
