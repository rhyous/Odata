using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Tests
{
    [DataContract]
    public class Smile
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "Type")]
        [JsonProperty("Type")]
        public string SmileType { get; set; }
    }
}
