namespace Rhyous.Odata.Tests
{
    [LookupEntity]
    public class EntityWithLookupDefault
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [LookupEntity(MaxCountToBehaveAsEnum = 10)]
    public class EntityWithLookupConfigured
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
