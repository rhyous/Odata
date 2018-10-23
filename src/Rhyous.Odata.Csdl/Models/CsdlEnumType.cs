using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract(Name = "Property")]
    public class CsdlEnumProperty
    {
        /// <summary>
        /// JSON Schema Kind. Should always be EntityType.
        /// </summary>
        [DataMember(Name = "$Kind")]
        public string Kind { get; set; } = "EnumType";

        /// <summary>
        /// Underlying type of the enum.
        /// </summary>
        [DataMember(Name = "$UnderlyingType", Order=3)]
        public string UnderlyingType { get; set; }

        /// <summary>
        /// Specifies whether this enum is a flag.
        /// </summary>
        [DataMember(Name = "$IsFlags", EmitDefaultValue = false)]
        public bool IsFlags { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> EnumOptions
        {
            get { return _EnumOptions ?? (_EnumOptions = new Dictionary<string, object>()); }
            set { _EnumOptions = value; }
        } private Dictionary<string, object> _EnumOptions;
    }
}