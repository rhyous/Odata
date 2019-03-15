using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    [DataContract]
    public class CsdlPath
    {
        /// <summary>
        /// Specifies whether this property is nullable.
        /// </summary>
        /// <remarks>See UI.DisplayName example in http://docs.oasis-open.org/odata/odata-csdl-json/v4.01/cs01/odata-csdl-json-v4.01-cs01.html#sec_VocabularyandAnnotation</remarks>
        [DataMember(Name = "$Path")]
        public string Path { get; set; }
    }
}
