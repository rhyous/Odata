using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlDocumentSerializationTests
    {
        [TestMethod]
        public void CsdlDocument_Serialization_User_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("User", typeof(User).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"User\":{\"$Kind\":\"EntityType\",\"$Key\":[\"Id\"],\"Id\":{\"$Type\":\"Edm.Int32\"},\"Name\":{\"$Type\":\"Edm.String\",\"@UI.Required\":true},\"UserTypeId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"UserType\"},\"UserType\":{\"$Type\":\"self.UserType\",\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"UserTypeId\":\"Id\"},\"@EAF.RelatedEntity.Type\":\"Local\"},\"UserRoles\":{\"$Type\":\"self.UserRole\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.Type\":\"Mapping\",\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserRoleMembership\"},\"UserGroups\":{\"$Type\":\"self.UserGroup\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.Type\":\"Mapping\",\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserGroupMembership\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlDocument_Serialization_User_ExcludeDefault_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("User", typeof(User).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"User\":{\"$Kind\":\"EntityType\",\"$Key\":[\"Id\"],\"Id\":{\"$Type\":\"Edm.Int32\"},\"Name\":{\"@UI.Required\":true},\"UserTypeId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"UserType\"},\"UserType\":{\"$Type\":\"self.UserType\",\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"UserTypeId\":\"Id\"},\"@EAF.RelatedEntity.Type\":\"Local\"},\"UserRoles\":{\"$Type\":\"self.UserRole\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.Type\":\"Mapping\",\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserRoleMembership\"},\"UserGroups\":{\"$Type\":\"self.UserGroup\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.Type\":\"Mapping\",\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserGroupMembership\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlDocument_Serialization_SuiteMembership_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            service.Entities.Add("SuiteMembership", typeof(SuiteMembership).ToCsdl());
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.Add("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"SuiteMembership\":{\"$Kind\":\"EntityType\",\"$Key\":[\"Id\"],\"Id\":{\"$Type\":\"Edm.Int32\"},\"ProductId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"Product\"},\"Product\":{\"$Type\":\"self.Product\",\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"ProductId\":\"Id\"},\"@EAF.RelatedEntity.Type\":\"Local\"},\"Quantity\":{\"$Type\":\"Edm.Double\"},\"QuantityType\":{\"$Kind\":\"EnumType\",\"$UnderlyingType\":\"Edm.Int32\",\"Inherited\":1,\"Fixed\":2,\"Percentage\":3},\"SuiteId\":{\"$Type\":\"Edm.Int32\"}}}}";

            // Act
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlDocument_Serialization_String_Test()
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