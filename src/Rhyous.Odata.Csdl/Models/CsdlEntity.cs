using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// This object contains schema information for an entity to be returned as csdl.
    /// </summary>
    /// <remarks>Needs to be rewritten accordin to this spec: http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/odata-csdl-json-v4.01.html 
    /// </remarks>

    [DataContract]
    public class CsdlEntity
    {
        /// <summary>
        /// JSON Schema Kind. Should always be EntityType.
        /// </summary>
        [DataMember(Name = CsdlConstants.Kind)]
        public string Kind { get; set; } = CsdlConstants.EntityType;

        /// <summary>
        /// Specifies whether this entity outputs data, such as a stream or image.
        /// </summary>        
        [DataMember(Name = CsdlConstants.HasStream, EmitDefaultValue = false)]
        public bool HasStream { get; set; }

        /// <summary>
        /// Additional JSON Schema object keyword.
        /// </summary>
        [DataMember(Name = CsdlConstants.OpenType, EmitDefaultValue = false)]
        public bool OpenType { get; set; }

        /// <summary>
        /// The key properties of this entity.
        /// </summary>
        [DataMember(Name = CsdlConstants.Key, EmitDefaultValue = false)]
        public List<string> Keys
        {
            get { return _Keys ?? (_Keys = new List<string>()); }
            set { _Keys = value; }
        } private List<string> _Keys;

        /// <summary>
        /// The properties of this entity.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, object> Properties
        {
            get { return _Properties ?? (_Properties = new Dictionary<string, object>()); }
            set { _Properties = value; }
        } private Dictionary<string, object> _Properties;
    }
}