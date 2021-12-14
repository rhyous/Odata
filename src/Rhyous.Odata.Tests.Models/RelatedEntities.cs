namespace Rhyous.Odata.Tests.Models
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
}
