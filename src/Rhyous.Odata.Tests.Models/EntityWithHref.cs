using Rhyous.Odata.Csdl;

namespace Rhyous.Odata.Tests
{
    public class EntityWithHref
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [HRef]
        public string Link { get; set; }
    }
}
