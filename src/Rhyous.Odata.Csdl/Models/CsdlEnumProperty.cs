using Newtonsoft.Json;
using Rhyous.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract]
    public class CsdlEnumProperty
    {
        /// <summary>
        /// JSON Schema Kind. Should always be EntityType.
        /// </summary>
        [DataMember(Name = CsdlConstants.Kind)]
        public string Kind { get; set; } = CsdlConstants.EnumType;

        /// <summary>
        /// Underlying type of the enum.
        /// </summary>
        [DataMember(Name = CsdlConstants.UnderlyingType, Order=3)]
        public string UnderlyingType { get; set; }

        /// <summary>
        /// Specifies whether this enum is a flag.
        /// </summary>
        [DataMember(Name = CsdlConstants.IsFlags, EmitDefaultValue = false)]
        public bool IsFlags { get; set; }

        /// <summary>
        /// This is used for listing all enum options as well as custom data.
        /// </summary>
        [JsonExtensionData]
        public SortedConcurrentDictionary<string, object> CustomData
        {
            get { return _CustomData ?? (_CustomData = new SortedConcurrentDictionary<string, object>()); }
            set { _CustomData = value; }
        } private SortedConcurrentDictionary<string, object> _CustomData;
    }
}