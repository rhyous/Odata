using Rhyous.Odata.Csdl;

namespace Rhyous.Odata.Tests
{
    public class EntityWithImageFile
    {
        [CsdlFileProperty(FileTypes.Image)]
        public byte[] Image { get; set; }
    }
}