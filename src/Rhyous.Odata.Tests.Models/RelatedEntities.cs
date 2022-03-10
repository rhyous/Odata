namespace Rhyous.Odata.Tests
{

    public class A
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity(nameof(B))]
        public int BId { get; set; }
    }

    [RelatedEntityForeign(nameof(A), nameof(B))]
    public class B
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class C
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity(nameof(D), EntityAlias = "X", RelatedEntityAlias = "Y")]
        public int DId { get; set; }
    }

    [RelatedEntityForeign(nameof(C), nameof(D), EntityAlias = "Y", RelatedEntityAlias = "X")]
    public class D
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
