using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    public class RelatedEntity : OdataObject<JRaw, string>
    {
        internal protected override void SetId(JRaw value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return;
            var jObj = JObject.Parse(value.ToString());
            Id = jObj.GetIdDynamic("IdProperty") ?? Id;
        }        

        /// <summary>
        /// The entity relationship type.
        /// </summary>
        public enum Type
        {
            OneToOne,        // Current entity has a foreign key or id property for the related entity and this foreign key or id property is unique.
            OneToOneForeign, // The related entity has a foreign key or id property for the current entity and this foreign key or id property is unique.
            OneToMany,       // The related entity has a foreign key or id property for current entity
            ManyToOne,       // Current entity has a foreign key or id property for the related entity
            ManyToMany       // Mapping table has mapping Id properties
        }
    }
}