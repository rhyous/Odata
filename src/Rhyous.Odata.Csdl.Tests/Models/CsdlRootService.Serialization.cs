﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CsdlRootService_Serialization_User_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("User", typeof(User).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"User\":{\"$Kind\":\"EntityType\",\"Id\":{\"$Type\":\"Edm.Int32\"},\"Name\":{\"$Collection\":true,\"$Type\":\"Edm.String\"},\"UserTypeId\":{\"$Type\":\"Edm.Int32\"},\"UserType\":{\"Type\":\"self.UserType\",\"Kind\":\"NavigationProperty\",\"IsCollection\":false,\"IsNullable\":false,\"Partner\":null,\"ContainsTarget\":false,\"ReferentialConstraint\":{\"UserTypeId\":\"Id\"},\"OnDelete\":\"None\"},\"UserRole\":{\"Type\":\"self.UserRole\",\"Kind\":\"NavigationProperty\",\"IsCollection\":true,\"IsNullable\":true,\"Partner\":null,\"ContainsTarget\":false,\"ReferentialConstraint\":null,\"OnDelete\":\"None\"},\"UserGroup\":{\"Type\":\"self.UserGroup\",\"Kind\":\"NavigationProperty\",\"IsCollection\":true,\"IsNullable\":true,\"Partner\":null,\"ContainsTarget\":false,\"ReferentialConstraint\":null,\"OnDelete\":\"None\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlRootService_Serialization_SuiteMembership_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("SuiteMembership", typeof(SuiteMembership).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"SuiteMembership\":{\"$Kind\":\"EntityType\",\"Id\":{\"$Type\":\"Edm.Int32\"},\"ProductId\":{\"$Type\":\"Edm.Int32\"},\"Product\":{\"Type\":\"self.Product\",\"Kind\":\"NavigationProperty\",\"IsCollection\":false,\"IsNullable\":false,\"Partner\":null,\"ContainsTarget\":false,\"ReferentialConstraint\":{\"ProductId\":\"Id\"},\"OnDelete\":\"None\"},\"Quantity\":{\"$Type\":\"Edm.Double\"},\"QuantityType\":{\"$Kind\":\"EnumType\",\"$UnderlyingType\":\"Edm.Int32\",\"Inherited\":1,\"Fixed\":2,\"Percentage\":3},\"SuiteId\":{\"$Type\":\"Edm.Int32\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlRootService_Serialization_String_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("User", JToken.Parse("{ \"Custom\": \"Json\" }"));
            var doc = new CsdlDocument { Version = "1.0", EntityContainer = "EAF" };
            doc.Schemas.Add("UserService", service);
            var expectedJson = "{\"$Version\":\"1.0\",\"$EntityContainer\":\"EAF\",\"UserService\":{\"$Alias\":\"self\",\"User\":{\"Custom\":\"Json\"}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }
    }
}