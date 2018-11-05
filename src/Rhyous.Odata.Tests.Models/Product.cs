namespace Rhyous.Odata.Tests
{
    [RelatedEntityForeign("Sku", "Product")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
