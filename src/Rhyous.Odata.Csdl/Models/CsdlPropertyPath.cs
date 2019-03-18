using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlPropertyPath
    {
        /// <summary>
        /// The path to a property.
        /// </summary>
        /// <remarks>See UI.DisplayName example in http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_PropertyPath</remarks>
        [DataMember(Name = "$PropertyPath")]
        public string PropertyPath { get; set; }
    }
}
