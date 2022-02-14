using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    public class EntityWithRequired
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
