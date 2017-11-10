﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata
{
    public static class OdataObjectExtensions
    {
        public static RelatedEntity AsRelatedEntity(this OdataObject o, RelatedEntity re = null)
        {
            re = re ?? new RelatedEntity();
            re.Id = o.Id.ToString();
            re.IdProperty = o.IdProperty;
            re.Object = o.Object;
            re.PropertyUris = o.PropertyUris;
            re.RelatedEntities = o.RelatedEntities;
            re.Uri = o.Uri;
            return re;
        }

        public static RelatedEntity AsRelatedEntity(this OdataObject<JRaw, string> o, RelatedEntity re = null)
        {
            re = re ?? new RelatedEntity();
            re.Id = o.Id.ToString();
            re.IdProperty = o.IdProperty;
            re.Object = o.Object;
            re.PropertyUris = o.PropertyUris;
            re.RelatedEntities = o.RelatedEntities;
            re.Uri = o.Uri;
            return re;
        }

        public static RelatedEntity AsRelatedEntity<TEntity, TId>(this OdataObject<TEntity, TId> obj, RelatedEntity re = null)
        {
            JRaw rawObj = obj.Object == null ? null : new JRaw(JsonConvert.SerializeObject(obj.Object));
            re = re ?? new RelatedEntity();
            re.Id = obj.Id.ToString();
            re.IdProperty = obj.IdProperty;
            re.Object = rawObj;
            re.PropertyUris = obj.PropertyUris;
            re.RelatedEntities = obj.RelatedEntities;
            re.Uri = obj.Uri;
            return re;
        }
    }
}