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

        [TestMethod]
        public void SettingObjectNullTest()
        {
            // Arrange
            var odataObj = new OdataObject<Entity1, int>();

            // Act
            odataObj.Object = null;

            // Assert
            Assert.IsNull(odataObj.Object);
        }

        [TestMethod]
        public void SettingObjectNullPreviousValueNotNullTest()
        {
            // Arrange
            var odataObj = new OdataObject<Entity1, int> { Object = new Entity1 { Id = 10 } };

            // Act
            odataObj.Object = null;

            // Assert
            Assert.IsNull(odataObj.Object);
            Assert.AreEqual(10, odataObj.Id, "Changing object to null doesn't alter the id.");
        }

        [TestMethod]
        public void CastObjNullTest()
        {
            // Arrange
            var odataObj = new OdataObject<Entity1, int>();

            // Act
            RelatedEntity re = odataObj;

            // Assert
            foreach (var prop in odataObj.GetType().GetProperties())
            {
                Assert.AreEqual(re.GetType().GetProperty(prop.Name).GetValue(re)?.ToString(), prop.GetValue(odataObj)?.ToString());
            }
        }

        [TestMethod]
        public void CastObjNotNullTest()
        {
            // Arrange
            var odataObj = new OdataObject<Entity1, int>();
            var entity1 = new Entity1 { Id = 10 };
            odataObj.Object = entity1;

            // Act
            RelatedEntity re = odataObj;

            // Assert
            foreach (var prop in odataObj.GetType().GetProperties())
            {
                if (prop.Name != "Object")
                    Assert.AreEqual(re.GetType().GetProperty(prop.Name).GetValue(re)?.ToString(), prop.GetValue(odataObj)?.ToString());
            }
        }
    }
}
