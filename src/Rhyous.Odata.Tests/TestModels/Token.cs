namespace Rhyous.Odata.Tests
{
    public partial class Token
    {
        public string Text { get; set; }
        
        [RelatedEntity("User", AutoExpand = true)]
        public long UserId { get; set; }
    }
}
