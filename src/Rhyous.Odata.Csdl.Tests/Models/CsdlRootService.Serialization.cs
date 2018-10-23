using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CsdlRootService_Serialization_User_Test()
        {
            // Arrange
            var service = new CsdlService();
            service.Entities.Add("User", typeof(Person).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Services.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"User\":{\"$Kind\":\"EntityType\",\"Id\":{\"$Type\":\"Edm.Int32\"},\"FirstName\":{\"$Collection\":true,\"$Type\":\"Edm.String\"},\"LastName\":{\"$Collection\":true,\"$Type\":\"Edm.String\"},\"DateOfBirth\":{\"$Type\":\"Edm.Date\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);

        }

        [TestMethod]
        public void CsdlRootService_Serialization_SuiteMembership_Test()
        {
            // Arrange
            var service = new CsdlService();
            service.Entities.Add("SuiteMembership", typeof(SuiteMembership).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Services.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"SuiteMembership\":{\"$Kind\":\"EntityType\",\"Id\":{\"$Type\":\"Edm.Int32\"},\"SuiteId\":{\"$Type\":\"Edm.Int32\"},\"ProductId\":{\"$Type\":\"Edm.Int32\"},\"Quantity\":{\"$Type\":\"Edm.Double\"},\"QuantityType\":{\"$Kind\":\"EnumType\",\"$UnderlyingType\":\"Edm.Int32\",\"Inherited\":1,\"Fixed\":2,\"Percentage\":3}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlRootService_Serialization_String_Test()
        {
            // Arrange
            var service = new CsdlService();
            service.Entities.Add("User", JToken.Parse("{ \"Custom\": \"Json\" }"));
            var doc = new CsdlDocument { Version = "1.0", EntityContainer = "EAF" };
            doc.Services.Add("UserService", service);
            var expectedJson = "{\"$Version\":\"1.0\",\"$EntityContainer\":\"EAF\",\"UserService\":{\"User\":{\"Custom\":\"Json\"}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }
    }
}