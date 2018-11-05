using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract()]
    public class CsdlDocument
    {
        [DataMember(Name = "$Version")]
        public string Version { get; set; }

        [DataMember(Name = "$EntityContainer")]
        public string EntityContainer { get; set; }

        [JsonExtensionData]
        public Dictionary<string, object> Schemas
        {
            get { return _Schemas ?? (_Schemas = new Dictionary<string, object>()); }
            set { _Schemas = value; }
        } private Dictionary<string, object> _Schemas;
    }
}