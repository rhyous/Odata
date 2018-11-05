using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class EntityAttributeDictionaryTests
    {
        #region GetDisplayProperty
        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_Null_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetDisplayProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetDisplayProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_DisplayColumnAttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetDisplayProperty(typeof(Smile));

            // Assert
            Assert.AreEqual("@UI.DisplayProperty", actual.First().Key);
            Assert.AreEqual("SmileType", actual.First().Value);
        }
        #endregion

        #region GetReadOnlyProperty
        [TestMethod]
        public void EntityAttributeDictionary_GetReadOnlyProperty_Null_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetReadOnlyProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetReadOnlyProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetReadOnlyProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetReadOnlyProperty_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetReadOnlyProperty(typeof(Country));

            // Assert
            Assert.AreEqual("@UI.ReadOnly", actual.First().Key);
            Assert.IsTrue((bool)actual.First().Value);
        }
        #endregion
        
        #region GetRelatedEntityForeignProperties

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_Null_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityForeignProperties(null);

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityForeignProperties(typeof(Person));

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityForeignProperties(typeof(Product));

            // Assert
            Assert.AreEqual("Sku", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_AttributeWithAliasExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityForeignProperties(typeof(EntityWithRelatedEntityAlias));

            // Assert
            Assert.AreEqual("E1", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }
        #endregion

        #region GetRelatedEntityMappingProperties

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_Null_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityMappingProperties(null);

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityMappingProperties(typeof(Person));

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityMappingProperties(typeof(User)).ToList();

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("UserRole", actual[0].Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual[0].Value.GetType());
            Assert.AreEqual("UserGroup", actual[1].Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual[1].Value.GetType());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_AttributeWithAliasExists_Test()
        {
            // Arrange
            // Act
            var actual = EntityAttributeDictionary.Instance.GetRelatedEntityMappingProperties(typeof(EntityWithRelatedEntityAlias));

            // Assert
            Assert.AreEqual("E2", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }
        #endregion
    }
}
