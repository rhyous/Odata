using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlSchema
    {

        [DataMember(Name = "$Alias")]
        public string Alias { get; set; } = "self";

        [JsonExtensionData]
        public Dictionary<string, object> Entities
        {
            get { return _Entities ?? (_Entities = new Dictionary<string, object>()); }
            set { _Entities = value; }
        } private Dictionary<string, object> _Entities;
    }
}