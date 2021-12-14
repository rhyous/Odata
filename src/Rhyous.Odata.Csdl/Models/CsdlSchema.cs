using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlSchema
    {
        [DataMember(Name = CsdlConstants.Alias)]
        public string Alias { get; set; } = CsdlConstants.DefaultSchemaOrAlias;

        [JsonExtensionData]
        public Dictionary<string, object> Entities
        {
            get { return _Entities ?? (_Entities = new Dictionary<string, object>()); }
            set { _Entities = value; }
        } private Dictionary<string, object> _Entities;
    }
}