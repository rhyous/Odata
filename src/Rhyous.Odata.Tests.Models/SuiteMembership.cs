namespace Rhyous.Odata.Tests
{
    public enum QuantityType
    {
        Inherited = 1,
        Fixed,
        Percentage
    }

    public class SuiteMembership
    {
        public int Id { get; set; }

        public int SuiteId { get; set; }

        [RelatedEntity("Product")]
        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public QuantityType QuantityType { get; set; }
    }
}
