using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    public class CsdlNavigationProperty
    {
        [DataMember(Name = "$Kind")]
        public string Type { get; set; }

        [DataMember(Name = "$Type")]
        public string Kind { get; set; } = "NavigationProperty";
        [DataMember(Name = "$Collection")]
        public bool IsCollection { get; set; }
        [DataMember(Name = "$Nullable")]
        public bool IsNullable { get; set; }
        [DataMember(Name = "$Partner")]
        public string Partner { get; set; }
        [DataMember(Name = "$ContainsTarget")]
        public bool ContainsTarget { get; set; }
        [DataMember(Name = "$ReferentialConstraint")]
        public object ReferentialConstraint { get; set; }
        [DataMember(Name = "$OnDelete")] // Cascade, None, SetNull, or SetDefault
        [JsonConverter(typeof(StringEnumConverter))]
        public OnDelete OnDelete { get; set; }
    }
}