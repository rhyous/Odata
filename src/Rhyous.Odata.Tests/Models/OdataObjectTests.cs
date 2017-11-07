using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class OdataObjectTests
    {
        public class Entity1 { public int Id { get; set; }}

        [TestMethod]
        public void SettingObjectSetsIdTest()
        {
            // Arrange
            var odataObj = new OdataObject<Entity1, int>();
            var entity1 = new Entity1 { Id = 10 };

            // Act
            odataObj.Object = entity1;

            // Assert
            Assert.AreEqual(10, odataObj.Id);
        }
    }
}
