namespace Rhyous.Odata.Tests
{
    [RelatedEntityForeign("SuiteMembership", "Product")] // Product (not Suite) to SuiteMembership
    [RelatedEntityForeign("SuiteMembership", "Product", RelatedEntityAlias = "ProductMembership", ForeignKeyProperty = "SuiteId")] // Suite to SuiteMembership
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", RelatedEntityAlias = "Suite")]
    [RelatedEntityMapping("Product", "SuiteMembership", "Product", EntityAlias = "Suite", MappingEntityAlias = "ProductMembership", RelatedEntityAlias = "ProductInSuite")]
    [RelatedEntityForeign("Sku", "Product")]
    public class Product2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
