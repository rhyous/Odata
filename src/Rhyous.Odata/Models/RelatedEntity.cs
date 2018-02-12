using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntity<TEntity, TId> : OdataObject<TEntity, TId>
    {       
        public static implicit operator RelatedEntity(RelatedEntity<TEntity, TId> re)
        {
            if (re == null)
                return null;
            return new RelatedEntity()
            {
                Id = re.Id?.ToString(),
                IdProperty = re.IdProperty,
                Object = new JRaw(JsonConvert.SerializeObject(re.Object)),
                Parent = re.Parent,
                PropertyUris = re.PropertyUris,
                RelatedEntityCollection = re.RelatedEntityCollection,
                Uri = re.Uri
            };
        }
    }
}