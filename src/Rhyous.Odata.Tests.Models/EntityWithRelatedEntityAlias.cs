namespace Rhyous.Odata.Tests
{
    [RelatedEntityForeign("Entity1", "EntityWithRelatedEntityAlias", RelatedEntityAlias = "E1")]

    [RelatedEntityMapping("Entity2", "FakeMap", "EntityWithRelatedEntityAlias", RelatedEntityAlias = "E2")]
    public class EntityWithRelatedEntityAlias
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity(nameof(Entity3), RelatedEntityAlias = "E3")]
        public int Entity3Id { get; set; }
    }

    public class Entity3
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
