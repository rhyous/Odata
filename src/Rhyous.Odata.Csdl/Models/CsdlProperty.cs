using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract(Name = "Property")]
    public class CsdlProperty
    {
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

        /// <summary>
        /// A positive integer value specifying the maximum length of a binary, stream 
        /// or string value. For binary or stream values this is the octet length of the
        /// binary data, for string values it is the character length. If no maximum 
        /// length is specified, clients SHOULD expect arbitrary length.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_MaxLength</remarks>
        [DataMember(Name = "$MaxLength", EmitDefaultValue = false)]
        public long MaxLength { get; set; }

        /// <summary>
        /// JSON Schema types.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Type</remarks>
        [DataMember(Name = "$Type", Order=1)]
        public virtual string Type { get; set; }

        [DataMember(Name = "$Precision", EmitDefaultValue = false)]
        public int Precision { get; set; }

        [DataMember(Name = "$Scale", EmitDefaultValue = false)]
        public int Scale { get; set; }

        [DataMember(Name = "$Unicode", EmitDefaultValue = false)]
        public bool IsUnicode { get; set; }

        [DataMember(Name = "$DefaultValue", EmitDefaultValue = false)]
        public object DefaultValue { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> CustomData
        {
            get { return _CustomData ?? (_CustomData = new Dictionary<string, object>()); }
            set { _CustomData = value; }
        } private Dictionary<string, object> _CustomData;
    }
}