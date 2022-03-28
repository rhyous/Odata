using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    public interface IName 
    {
        [StringLength(100, MinimumLength = 4)]
        string Name { get; set; }
    }
    public class EntityWithName : IName
    {
        public string Name { get; set; }
    }
}