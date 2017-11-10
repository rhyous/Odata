using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntity<TEntity, TId> : OdataObject<TEntity, TId>
    {
        public static implicit operator RelatedEntity(RelatedEntity<TEntity, TId> re)
        {
            return new RelatedEntity()
            {
                Id = re.Id.ToString(),
                IdProperty = re.IdProperty,
                Object = new JRaw(JsonConvert.SerializeObject(re.Object)),
                PropertyUris = re.PropertyUris,
                RelatedEntities = re.RelatedEntities,
                Uri = re.Uri
            };
        }
    }
}