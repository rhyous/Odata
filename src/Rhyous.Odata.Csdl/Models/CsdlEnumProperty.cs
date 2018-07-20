using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract(Name = "Property")]
    public class CsdlEnumProperty : CsdlProperty
    {
        /// <summary>
        /// Underlying type of the enum.
        /// </summary>
        [DataMember(Name = "$UnderlyingType", Order=3)]
        public string UnderlyingType { get; set; }
    }
}