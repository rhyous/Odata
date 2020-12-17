namespace Rhyous.Odata.Tests
{
    public class EntityWithDisplayCondition
    {
        public string Id { get; set; }
        [RelatedEntity("Type", DisplayCondition = "Type eq 1")]
        public int TypeId { get; set; }
    }
}