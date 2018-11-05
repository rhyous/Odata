using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    [DisplayColumn("Text")]
    public partial class Token
    {
        public string Text { get; set; }
        
        [RelatedEntity("User", AutoExpand = true)]
        public long UserId { get; set; }
    }
}
