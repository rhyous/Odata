using Newtonsoft.Json;
using Rhyous.Odata.Csdl;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Tests
{
    public class EntityExcludeFromMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ExcludeFromMetadata]
        public string Value1 { get; set; }
        [JsonIgnore]
        public string Value2 { get; set; }
        [IgnoreDataMember]
        public string Value3 { get; set; }
    }
}
