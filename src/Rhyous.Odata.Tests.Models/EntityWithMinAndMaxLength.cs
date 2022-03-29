using Rhyous.Odata.Csdl;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    public class EntityWithMinAndMaxLengthAttributes
    {
        [MaxLength(10)]
        [MinLength(2)]
        public string Name { get; set; }
    }

    public class EntityWithMinAndMaxLengthInStringLengthAttribute
    {
        [StringLength(10, MinimumLength = 2)]
        public string Name { get; set; }
    }
    public class EntityWithMinAndMaxLengthInCsdlPropertyAttribute
    {
        [CsdlProperty(MinLength = 2, MaxLength = 10)]
        public string Name { get; set; }
    }
}