using Newtonsoft.Json;
using Rhyous.Collections;
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
        public SortedConcurrentDictionary<string, object> Entities
        {
            get { return _Entities ?? (_Entities = new SortedConcurrentDictionary<string, object>()); }
            set { _Entities = value; }
        } private SortedConcurrentDictionary<string, object> _Entities;
    }
}