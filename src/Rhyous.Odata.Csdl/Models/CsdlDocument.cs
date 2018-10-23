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
        public Dictionary<string, object> Services
        {
            get { return _Services ?? (_Services = new Dictionary<string, object>()); }
            set { _Services = value; }
        } private Dictionary<string, object> _Services;
    }
}