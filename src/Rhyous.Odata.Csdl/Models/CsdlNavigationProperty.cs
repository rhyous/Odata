using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Rhyous.Collections;
using System.Collections.Generic;
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
        [DefaultValue(CsdlConstants.EdmString)]
        [DataMember(Name = CsdlConstants.Type)]
        public virtual string Type { get; set; }

        /// <summary>
        /// JSON Schema alis types.
        /// </summary>
        [DataMember(Name = CsdlConstants.Alias, EmitDefaultValue = false)]
        public virtual string Alias { get; set; }

        /// <summary>
        /// JSON Schema Kind. Should always be EntityType.
        /// </summary>
        [DataMember(Name = CsdlConstants.Kind)]
        public string Kind { get; set; } = CsdlConstants.NavigationProperty;

        /// <summary>
        /// Specifies whether this property is nullable.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Nullable</remarks>
        [DataMember(Name = CsdlConstants.Nullable, EmitDefaultValue = false)]
        public bool Nullable { get; set; }

        /// <summary>
        /// Specifies whether this property is a collection.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Type</remarks>
        [DataMember(Name = CsdlConstants.Collection, EmitDefaultValue = false)]
        public bool IsCollection { get; set; }

        [DataMember(Name = CsdlConstants.Partner, EmitDefaultValue = false)]
        public string Partner { get; set; }

        [DataMember(Name = CsdlConstants.ContainsTarget, EmitDefaultValue = false)]
        public bool ContainsTarget { get; set; }

        [DataMember(Name = CsdlConstants.ReferentialConstraint, EmitDefaultValue = false)]
        public CsdlReferentialConstraint ReferentialConstraint { get; set; }

        /// <summary>
        /// None, Cascade, SetNull, or SetDefault
        /// </summary>
        [DefaultValue(OnDelete.None)]
        [DataMember(Name = CsdlConstants.OnDelete, EmitDefaultValue = false)] 
        [JsonConverter(typeof(StringEnumConverter))]
        public OnDelete OnDelete { get; set; }

        [JsonExtensionData]
        public SortedConcurrentDictionary<string, object> CustomData
        {
            get { return _CustomData ?? (_CustomData = new SortedConcurrentDictionary<string, object>()); }
            set { _CustomData = value; }
        } private SortedConcurrentDictionary<string, object> _CustomData;
    }
}