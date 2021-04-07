using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Rhyous.StringLibrary;
using System;

namespace Rhyous.Odata
{
    public static class OdataObjectExtensions
    {
        public static RelatedEntity AsRelatedEntity(this OdataObject o, RelatedEntity re = null)
        {
            if (o == null)
                return null;
            re = re ?? new RelatedEntity();
            re.Id = o.Id?.ToString();
            re.IdProperty = o.IdProperty;
            re.Object = o.Object;
            re.Parent = re.Parent;
            re.PropertyUris = o.PropertyUris;
            re.RelatedEntityCollection = o.RelatedEntityCollection;
            re.Uri = o.Uri;
            return re;
        }

        public static RelatedEntity AsRelatedEntity(this OdataObject<JRaw, string> o, RelatedEntity re = null)
        {
            if (o == null)
                return null;
            re = re ?? new RelatedEntity();
            re.Id = o.Id?.ToString();
            re.IdProperty = o.IdProperty;
            re.Object = o.Object;
            re.Parent = re.Parent;
            re.PropertyUris = o.PropertyUris;
            re.RelatedEntityCollection = o.RelatedEntityCollection;
            re.Uri = o.Uri;
            return re;
        }

        public static RelatedEntity AsRelatedEntity<TEntity, TId>(this OdataObject<TEntity, TId> obj, RelatedEntity re = null)
        {
            if (obj == null)
                return null;
            JRaw rawObj = obj.Object == null ? null : new JRaw(JsonConvert.SerializeObject(obj.Object));
            re = re ?? new RelatedEntity();
            re.Id = obj.Id?.ToString();
            re.IdProperty = obj.IdProperty;
            re.Object = rawObj;
            re.Parent = re.Parent;
            re.PropertyUris = obj.PropertyUris;
            re.RelatedEntityCollection = obj.RelatedEntityCollection;
            re.Uri = obj.Uri;
            return re;
        }

        public static OdataObject<TEntity, TId> ToOdataObject<TEntity, TId>(this OdataObject obj)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>
        {
            if (obj == null)
                return null;
            var retObj = new OdataObject<TEntity, TId>();
            retObj.Id = obj.Id.To<TId>();
            retObj.IdProperty = obj.IdProperty;
            retObj.Object = JsonConvert.DeserializeObject<TEntity>(obj.Object.ToString());
            retObj.PropertyUris = obj.PropertyUris;
            retObj.RelatedEntityCollection = obj.RelatedEntityCollection;
            retObj.Uri = obj.Uri;
            return retObj;
        }

        public static OdataObject ToOdataObject(this OdataObject<JRaw, string> obj)
        {
            if (obj == null)
                return null;
            var retObj = new OdataObject();
            retObj.Id = obj.Id;
            retObj.IdProperty = obj.IdProperty;
            retObj.Object = obj.Object;
            retObj.PropertyUris = obj.PropertyUris;
            retObj.RelatedEntityCollection = obj.RelatedEntityCollection;
            retObj.Uri = obj.Uri;
            return retObj;
        }

        public static OdataObject<TEntity, TId> ToOdataObject<TEntity, TId>(this OdataObject<JRaw, string> obj)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>
        {
            if (obj == null)
                return null;
            var retObj = new OdataObject<TEntity, TId>();
            retObj.Id = obj.Id.To<TId>();
            retObj.IdProperty = obj.IdProperty;
            retObj.Object = JsonConvert.DeserializeObject<TEntity>(obj.Object.ToString());            
            retObj.PropertyUris = obj.PropertyUris;
            retObj.RelatedEntityCollection = obj.RelatedEntityCollection;
            retObj.Uri = obj.Uri;
            return retObj;
        }

        /// <summary>
        /// A method to easily get the related entity.
        /// </summary>
        /// <typeparam name="TRelatedEntity"></typeparam>
        /// <typeparam name="TRelatedEntityId"></typeparam>
        /// <param name="odataObj"></param>
        /// <returns></returns>
        public static OdataObjectCollection<TRelatedEntity, TRelatedEntityId> GetRelatedEntityCollection<TRelatedEntity, TRelatedEntityId>(this IRelatedEntityCollection odataObj, string entityAlias = null)
            where TRelatedEntityId : IComparable<TRelatedEntityId>, IComparable, IEquatable<TRelatedEntityId>
        {
            if (odataObj == null || odataObj.RelatedEntityCollection == null || !odataObj.RelatedEntityCollection.Any())
                return null;
            var relatedEntity = entityAlias ?? typeof(TRelatedEntity).Name;
            var relatedEntityCollection = odataObj.RelatedEntityCollection.FirstOrDefault(re => re.RelatedEntity == relatedEntity);
            if (relatedEntityCollection == null)
                return null;
            var collection = new OdataObjectCollection<TRelatedEntity, TRelatedEntityId>();
            foreach (var item in relatedEntityCollection)
            {
                var obj = JsonConvert.DeserializeObject<TRelatedEntity>(item.Object.ToString());
                var odataItem = item.ToOdataObject<TRelatedEntity, TRelatedEntityId>();
                collection.Add(odataItem);
            }
            return collection;
        }
    }
}