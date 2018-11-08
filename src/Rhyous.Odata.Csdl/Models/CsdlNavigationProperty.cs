using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlNavigationProperty
    {
        /// <summary>
        /// JSON Schema types.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Type</remarks>
        [DefaultValue("Edm.String")]
        [DataMember(Name = "$Type")]
        public virtual string Type { get; set; }

        /// <summary>
        /// JSON Schema Kind. Should always be EntityType.
        /// </summary>
        [DataMember(Name = "$Kind")]
        public string Kind { get; set; } = "NavigationProperty";

        /// <summary>
        /// Specifies whether this property is nullable.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Nullable</remarks>
        [DataMember(Name = "$Nullable", EmitDefaultValue = false)]
        public bool Nullable { get; set; }

        /// <summary>
        /// Specifies whether this property is a collection.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Type</remarks>
        [DataMember(Name = "$Collection", EmitDefaultValue = false)]
        public bool IsCollection { get; set; }

        [DataMember(Name = "$Partner", EmitDefaultValue = false)]
        public string Partner { get; set; }

        [DataMember(Name = "$ContainsTarget", EmitDefaultValue = false)]
        public bool ContainsTarget { get; set; }

        [DataMember(Name = "$ReferentialConstraint", EmitDefaultValue = false)]
        public object ReferentialConstraint { get; set; }

        /// <summary>
        /// None, Cascade, SetNull, or SetDefault
        /// </summary>
        [DefaultValue(OnDelete.None)]
        [DataMember(Name = "$OnDelete", EmitDefaultValue = false)] 
        [JsonConverter(typeof(StringEnumConverter))]
        public OnDelete OnDelete { get; set; }
    }
}