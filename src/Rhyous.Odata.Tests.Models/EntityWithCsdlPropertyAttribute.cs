using Rhyous.Odata.Csdl;
using System.Drawing;

namespace Rhyous.Odata.Tests.Models
{
    public class EntityWithCsdlPropertyAttribute
    {
        public int Id { get; set; }

        [CsdlProperty(DefaultValue = -1)]
        public int DefaultValue { get; set; }

        [CsdlProperty(ExcludeFromMetadata = true)]
        public int Excluded { get; set; }

        [CsdlProperty(Required = false)]
        public int NotRequired { get; set; }

        [CsdlProperty(Required = true)]
        public int? RequiredDespiteBeingNullable { get; set; }

        [CsdlProperty(Nullable = false)]
        public int? ForceNullableToNotBeNullable { get; set; }

        [CsdlProperty(Required = false)]
        public int? NotRequiredNoMetadata { get; set; }

        [CsdlProperty(Required = true)]
        public int RequiredNoMetadata { get; set; }

        [CsdlProperty(MinLength = 5)]
        public string MinLength { get; set; }

        [CsdlProperty(MaxLength = 10)]
        public string MaxLength { get; set; }

        [CsdlProperty(Nullable = true)]
        public int Entity1Id { get; set; }

        [CsdlProperty(ReadOnly = true)]
        public int ReadOnly { get; set; }

        [CsdlProperty(CsdlType = "Edm.Password")]
        public string Password { get; set; }
    }
}
