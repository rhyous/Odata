using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    [LookupEntity]
    public interface IEntityWithInterfaceAttribute
    {
        int Id { get; set; }

        [StringLength(100)]
        string Name { get; set; }
    }

    public class EntityWithInterfaceAttribute : IEntityWithInterfaceAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}