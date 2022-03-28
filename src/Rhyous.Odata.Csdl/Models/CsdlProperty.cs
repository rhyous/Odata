using Newtonsoft.Json;
using Rhyous.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract]
    public class CsdlProperty
    {
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

        /// <summary>
        /// A positive integer value specifying the minimum length of a binary, stream 
        /// or string value. For binary or stream values this is the octet length of the
        /// binary data, for string values it is the character length. If no minimum 
        /// length is specified, clients SHOULD expect arbitrary length.
        /// </summary>
        /// <remarks>There was no MinLength in OData spec so I copied MaxLength.
        /// http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_MaxLength</remarks>
        [DataMember(Name = CsdlConstants.MinLength, EmitDefaultValue = false)]
        public ulong MinLength { get; set; }

        /// <summary>
        /// A positive integer value specifying the maximum length of a binary, stream 
        /// or string value. For binary or stream values this is the octet length of the
        /// binary data, for string values it is the character length. If no maximum 
        /// length is specified, clients SHOULD expect arbitrary length.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_MaxLength</remarks>
        [DataMember(Name = CsdlConstants.MaxLength, EmitDefaultValue = false)]
        public ulong MaxLength { get; set; }

        /// <summary>
        /// JSON Schema types.
        /// </summary>
        /// <remarks>http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_Type</remarks>
        [DefaultValue(CsdlConstants.EdmString)]
        [DataMember(Name = CsdlConstants.Type)]
        public virtual string Type { get; set; }

        [DataMember(Name = CsdlConstants.Precision, EmitDefaultValue = false)]
        public uint Precision { get; set; }

        [DataMember(Name = CsdlConstants.Scale, EmitDefaultValue = false)]
        public int Scale { get; set; }

        [DataMember(Name = CsdlConstants.Unicode, EmitDefaultValue = false)]
        public bool IsUnicode { get; set; }

        [DataMember(Name = CsdlConstants.DefaultValue, EmitDefaultValue = false)]
        public object DefaultValue { get; set; }

        [JsonExtensionData]
        public SortedConcurrentDictionary<string, object> CustomData
        {
            get { return _CustomData ?? (_CustomData = new SortedConcurrentDictionary<string, object>()); }
            set { _CustomData = value; }
        } private SortedConcurrentDictionary<string, object> _CustomData;
    }
}