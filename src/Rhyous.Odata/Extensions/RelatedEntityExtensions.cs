using Newtonsoft.Json;
using Rhyous.StringLibrary;
using System;

namespace Rhyous.Odata
{
    public static class RelatedEntityExtensions
    {
        public static OdataObject<TEntity, TId> ToOdataObject<TEntity, TId>(this RelatedEntity obj)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>
        {
            if (obj == null)
                return null;
            var retObj = new OdataObject<TEntity, TId>();
            retObj.Id = obj.Id.To<TId>();
            retObj.IdProperty = obj.IdProperty;
            retObj.Object = obj.Object == null ? default : JsonConvert.DeserializeObject<TEntity>(obj.Object.ToString());
            retObj.PropertyUris = obj.PropertyUris;
            retObj.RelatedEntityCollection = obj.RelatedEntityCollection;
            retObj.Uri = obj.Uri;
            return retObj;
        }
    }
}