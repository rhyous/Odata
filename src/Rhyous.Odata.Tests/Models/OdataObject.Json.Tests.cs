using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class OdataObjectJsonTests
    {
        public class Entity1 { public int Id { get; set; }}

        [TestMethod]
        public void SettingObjectSetsIdTest()
        {
            // Arrange
            var odataObj = new OdataObject();
            var entity1 = new JRaw(JsonConvert.SerializeObject(new Entity1 { Id = 10 }));

            // Act
            odataObj.Object = entity1;

            // Assert
            Assert.AreEqual("10", odataObj.Id);
        }

        [TestMethod]
        public void SettingObjectNullTest()
        {
            // Arrange
            var odataObj = new OdataObject();

            // Act
            odataObj.Object = null;

            // Assert
            Assert.IsNull(odataObj.Object);
        }

        [TestMethod]
        public void SettingObjectNullPreviousValueNotNullTest()
        {
            // Arrange
            var odataObj = new OdataObject { Object = new JRaw(JsonConvert.SerializeObject(new Entity1 { Id = 10 })) };

            // Act
            odataObj.Object = null;

            // Assert
            Assert.IsNull(odataObj.Object);
            Assert.AreEqual("10", odataObj.Id, "Changing object to null doesn't alter the id.");
        }

        [TestMethod]
        public void ImplicitCastDefaultObjectTest()
        {
            // Arrange
            var odataObj = new OdataObject();

            // Act
            RelatedEntity re = odataObj;

            // Assert
            foreach (var prop in odataObj.GetType().GetProperties())
            {
                Assert.AreEqual(re.GetType().GetProperty(prop.Name).GetValue(re)?.ToString(), prop.GetValue(odataObj)?.ToString());
            }
        }

        [TestMethod]
        public void ImplicitCastObjectPopulatedTest()
        {
            // Arrange
            var odataObj = new OdataObject();
            var entity1 = new Entity1 { Id = 10 };
            odataObj.Object = new JRaw(JsonConvert.SerializeObject(entity1));

            // Act
            RelatedEntity re = odataObj;

            // Assert
            foreach (var prop in odataObj.GetType().GetProperties())
            {
                if (prop.Name != "Object")
                    Assert.AreEqual(re.GetType().GetProperty(prop.Name).GetValue(re)?.ToString(), prop.GetValue(odataObj)?.ToString());
            }
        }

        [TestMethod]
        public void ImplicitOperatorNullTest()
        {
            // Arrange
            OdataObject<Entity1, int> o = null;

            // Act
            RelatedEntity e = o;

            // Assert
            Assert.IsNull(e);
        }
    }
}