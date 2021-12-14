using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    /// <summary>A model for Referential Constraints used to map entities and related entities.</summary>
    public class CsdlReferentialConstraint
    {
        public string LocalProperty { get; set; }
        public string ForeignProperty { get; set; }
        [JsonExtensionData]
        public Dictionary<string, object> CustomData
        {
            get { return _CustomData ?? (_CustomData = new Dictionary<string, object>()); }
            set { _CustomData = value; }
        } private Dictionary<string, object> _CustomData;
    }
}