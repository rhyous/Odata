namespace Rhyous.Odata.Tests
{
    public class EntityWithDuplicateRelatedEntityOneAlias
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity("Entity3")]
        [RelatedEntity("Entity3", RelatedEntityAlias = "E3")]
        public int Entity3Id { get; set; }
    }
}
