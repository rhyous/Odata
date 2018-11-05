using Rhyous.Odata.Csdl;

namespace Rhyous.Odata.Tests
{
    [ReadOnlyEntity]
    public class Country
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
    }
}
