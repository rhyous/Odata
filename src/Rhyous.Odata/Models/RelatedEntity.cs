﻿using System;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntity : OdataObject<JRaw, string>
    {
        internal protected override void SetId(JRaw value)
        {
            var jObj = JObject.Parse(value.ToString());
            var idProp = jObj.GetValue("IdProperty")?.ToString();
            if (string.IsNullOrWhiteSpace(idProp))
                idProp = "Id";
            Id = jObj.GetValue(idProp)?.ToString();
            if (string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("The object must have an Id property or the IdProperty must be specified.");
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
