namespace Rhyous.Odata.Tests
{
    public class EntityWithFilter
    {
        public string Id { get; set; }
        [RelatedEntity("Type", Filter = "Type eq 1")]
        public int TypeId { get; set; }
    }
}