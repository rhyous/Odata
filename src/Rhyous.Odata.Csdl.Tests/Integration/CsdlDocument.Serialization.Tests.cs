using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Rhyous.Collections;
using Rhyous.Odata.Tests;
using Rhyous.UnitTesting;
using System;
using System.Linq;

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
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.TryAdd("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"User\":{\"$Kind\":\"EntityType\",\"$Key\":[\"Id\"],\"Id\":{\"$Type\":\"Edm.Int32\"},\"Name\":{\"$Type\":\"Edm.String\"},\"UserGroups\":{\"$Type\":\"self.UserGroup\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserGroupMembership\",\"@EAF.RelatedEntity.Type\":\"Mapping\"},\"UserRoles\":{\"$Type\":\"self.UserRole\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserRoleMembership\",\"@EAF.RelatedEntity.Type\":\"Mapping\"},\"UserType\":{\"$Type\":\"self.UserType\",\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"LocalProperty\":\"UserTypeId\",\"ForeignProperty\":\"Id\",\"UserTypeId\":\"Id\"},\"@EAF.RelatedEntity.Type\":\"Local\"},\"UserTypeId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"UserType\"}}}}";
            service.Entities.TryAdd("User", typeof(User).ToCsdl());

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
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            doc.Schemas.TryAdd("EAF", service);
            var expectedJson = "{\"$Version\":\"4.01\",\"$EntityContainer\":\"EAF\",\"EAF\":{\"$Alias\":\"self\",\"User\":{\"$Kind\":\"EntityType\",\"$Key\":[\"Id\"],\"Id\":{\"$Type\":\"Edm.Int32\"},\"Name\":{},\"UserGroups\":{\"$Type\":\"self.UserGroup\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserGroupMembership\",\"@EAF.RelatedEntity.Type\":\"Mapping\"},\"UserRoles\":{\"$Type\":\"self.UserRole\",\"$Kind\":\"NavigationProperty\",\"$Nullable\":true,\"$Collection\":true,\"@EAF.RelatedEntity.MappingEntityType\":\"self.UserRoleMembership\",\"@EAF.RelatedEntity.Type\":\"Mapping\"},\"UserType\":{\"$Type\":\"self.UserType\",\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"LocalProperty\":\"UserTypeId\",\"ForeignProperty\":\"Id\",\"UserTypeId\":\"Id\"},\"@EAF.RelatedEntity.Type\":\"Local\"},\"UserTypeId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"UserType\"}}}}";

            // Act
            service.Entities.TryAdd("User", typeof(User).ToCsdl());
            var json = JsonConvert.SerializeObject(doc, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlDocument_Serialization_SuiteMembership_Test()
        {
            // Arrange
            var doc = new CsdlDocument { Version = "4.01", EntityContainer = "EAF" };
            var service = new CsdlSchema();
            service.Entities.TryAdd("SuiteMembership", typeof(SuiteMembership).ToCsdl());
            doc.Schemas.TryAdd("EAF", service);

            var expectedJson = "{\"$EntityContainer\":\"EAF\",\"$Version\":\"4.01\",\"EAF\":{\"$Alias\":\"self\",\"SuiteMembership\":{\"$Key\":[\"Id\"],\"$Kind\":\"EntityType\",\"Id\":{\"$Type\":\"Edm.Int32\"},\"Product\":{\"$Kind\":\"NavigationProperty\",\"$ReferentialConstraint\":{\"ForeignProperty\":\"Id\",\"LocalProperty\":\"ProductId\",\"ProductId\":\"Id\"},\"$Type\":\"self.Product\",\"@EAF.RelatedEntity.Type\":\"Local\"},\"ProductId\":{\"$Type\":\"Edm.Int32\",\"$NavigationKey\":\"Product\"},\"Quantity\":{\"$Type\":\"Edm.Double\"},\"QuantityType\":{\"$UnderlyingType\":\"Edm.Int32\",\"$Kind\":\"EnumType\",\"$Type\":\"Edm.Enum\",\"Fixed\":2,\"Inherited\":1,\"Percentage\":3},\"SuiteId\":{\"$Type\":\"Edm.Int32\"}}}}";

            var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new OrderedContractResolver() };

            // Act
            var json = JsonConvert.SerializeObject(doc, jsonSerializerSettings);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        public void CsdlDocument_Serialization_String_Test()
        {
            // Arrange
            var service = new CsdlSchema();
            var doc = new CsdlDocument { Version = "1.0", EntityContainer = "EAF" };
            doc.Schemas.TryAdd("UserService", service);
            var expectedJson = "{\"$Version\":\"1.0\",\"$EntityContainer\":\"EAF\",\"UserService\":{\"$Alias\":\"self\",\"User\":{\"Custom\":\"Json\"}}}";

            // Act
            service.Entities.TryAdd("User", JToken.Parse("{ \"Custom\": \"Json\" }"));
            var json = JsonConvert.SerializeObject(doc);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }

        [TestMethod]
        [PrimitiveList(typeof(EntityWithMinAndMaxLengthAttributes),
                       typeof(EntityWithMinAndMaxLengthInCsdlPropertyAttribute),
                       typeof(EntityWithMinAndMaxLengthInStringLengthAttribute))]
        public void CsdlProperty_Serialization_MinAndMaxLength_Tests(Type type)
        {
            // Arrange
            var propertyBuilder = CsdlBuilderFactory.Instance.PropertyBuilder;
            var expectedJson = "{\"$Nullable\":true,\"$MinLength\":2,\"$MaxLength\":10,\"$Type\":\"Edm.String\"}";
            var propInfo = type.GetProperty("Name");
            var csdlProperty = propertyBuilder.Build(propInfo);

            // Act
            var json = JsonConvert.SerializeObject(csdlProperty);

            // Assert
            Assert.AreEqual(expectedJson, json);
        }
    }

    public class OrderedContractResolver : DefaultContractResolver
    {
        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            var @base = base.CreateProperties(type, memberSerialization);
            var ordered = @base
                .OrderBy(p => p.Order ?? int.MaxValue)
                .ThenBy(p => p.PropertyName)
                .ToList();
            return ordered;
        }
    }
}