using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlPath
    {
        /// <summary>
        /// The path to a value expression.
        /// </summary>
        /// <remarks>See example in http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_ValuePath</remarks>
        [DataMember(Name = CsdlConstants.Path)]
        public string Path { get; set; }
    }
}
