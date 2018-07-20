namespace Rhyous.Odata.Tests
{
    public enum CoolType
    {
        Awesome,
        Cool
    }

    public class EntityWithEnum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CoolType Type { get; set; }
    }
}
