using Newtonsoft.Json;

namespace Rhyous.Odata.Tests
{
    public class Smile
    {
        public int Id { get; set; }

        [JsonProperty("Type")]
        public string SmileType { get; set; }
    }
}
