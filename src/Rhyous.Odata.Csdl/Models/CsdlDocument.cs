using Newtonsoft.Json;
using Rhyous.Collections;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract()]
    public class CsdlDocument
    {
        [DataMember(Name = CsdlConstants.Version)]
        public string Version { get; set; }

        [DataMember(Name = CsdlConstants.EntityContainer)]
        public string EntityContainer { get; set; }

        [JsonExtensionData]
        public SortedConcurrentDictionary<string, object> Schemas
        {
            get { return _Schemas ?? (_Schemas = new SortedConcurrentDictionary<string, object>()); }
            set { _Schemas = value; }
        } private SortedConcurrentDictionary<string, object> _Schemas;
    }
}