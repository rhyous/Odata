using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    public class RelatedEntityCollection
    {
        /// <summary>
        /// The current entity name, not the related entity name.
        /// </summary>
        [IgnoreDataMember]
        public string Entity { get; set; }

        /// <summary>
        /// The current entity Id or Key, not the related entity Id or Key.
        /// </summary>
        [IgnoreDataMember]
        public string EntityId { get; set; }

        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public string RelatedEntity { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        public List<RelatedEntity> Entities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntity>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntity> _RelatedEntities;
    }
}
