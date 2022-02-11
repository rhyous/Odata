using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Tests;
using System;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class EntityAttributeDictionaryTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityForeignNavigationPropertyBuilder> _MockRelatedEntityForeignNavigationPropertyBuilder;
        private Mock<IRelatedEntityMappingNavigationPropertyBuilder> _MockRelatedEntityMappingNavigationPropertyBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityForeignNavigationPropertyBuilder = _MockRepository.Create<IRelatedEntityForeignNavigationPropertyBuilder>();
            _MockRelatedEntityMappingNavigationPropertyBuilder = _MockRepository.Create<IRelatedEntityMappingNavigationPropertyBuilder>();
        }

        private EntityAttributeDictionary CreateEntityAttributeDictionary()
        {
            return new EntityAttributeDictionary(
                _MockRelatedEntityForeignNavigationPropertyBuilder.Object,
                _MockRelatedEntityMappingNavigationPropertyBuilder.Object);
        }

        #region GetDisplayProperty
        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_Null_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetDisplayProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetDisplayProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetDisplayProperty_DisplayColumnAttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetDisplayProperty(typeof(Smile));

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
            var actual = CreateEntityAttributeDictionary().GetReadOnlyProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetReadOnlyProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetReadOnlyProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetReadOnlyProperty_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetReadOnlyProperty(typeof(Country));

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
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityForeignProperties(null);

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityForeignProperties(typeof(Person));

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_AttributeExists_Test()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            var navigationProperty = new CsdlNavigationProperty();
            _MockRelatedEntityForeignNavigationPropertyBuilder.Setup(m => m.Build(It.IsAny<RelatedEntityForeignAttribute>(), "self"))
                                                              .Returns(navigationProperty);

            // Act
            var actual = entityAttributeDictionary.GetRelatedEntityForeignProperties(typeof(Product));

            // Assert
            Assert.AreEqual("Skus", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityForeignProperties_AttributeWithAliasExists_Test()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            var csdlNav = new CsdlNavigationProperty();
            _MockRelatedEntityForeignNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityForeignAttribute>(rea => rea.Entity == "EntityWithRelatedEntityAlias"),
                                                                                                                              CsdlConstants.DefaultSchemaOrAlias))
                                                              .Returns(csdlNav);

            // Act
            var actual = entityAttributeDictionary.GetRelatedEntityForeignProperties(typeof(EntityWithRelatedEntityAlias));

            // Assert
            Assert.AreEqual("E1s", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }
        #endregion

        #region GetRelatedEntityMappingProperties

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_Null_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityMappingProperties(null);

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityMappingProperties(typeof(Person));

            // Assert
            Assert.IsFalse(actual.Any());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_AttributeExists_Test()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            var csdlNav1 = new CsdlNavigationProperty();
            _MockRelatedEntityMappingNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityMappingAttribute>(rea => rea.RelatedEntity == "UserRole"),
                                                                                                                              CsdlConstants.DefaultSchemaOrAlias))
                                                              .Returns(csdlNav1);
            var csdlNav2 = new CsdlNavigationProperty();
            _MockRelatedEntityMappingNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityMappingAttribute>(rea => rea.RelatedEntity == "UserGroup"),
                                                                                                                              CsdlConstants.DefaultSchemaOrAlias))
                                                              .Returns(csdlNav1);

            // Act
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityMappingProperties(typeof(User)).ToList();

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("UserRoles", actual[0].Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual[0].Value.GetType());
            Assert.AreEqual("UserGroups", actual[1].Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual[1].Value.GetType());
        }

        [TestMethod]
        public void EntityAttributeDictionary_GetRelatedEntityMappingProperties_AttributeWithAliasExists_Test()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            var csdlNav1 = new CsdlNavigationProperty();
            _MockRelatedEntityMappingNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityMappingAttribute>(rea => rea.RelatedEntity == "Entity1"),
                                                                                                                              CsdlConstants.DefaultSchemaOrAlias))
                                                              .Returns(csdlNav1);
            var csdlNav2 = new CsdlNavigationProperty();
            _MockRelatedEntityMappingNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityMappingAttribute>(rea => rea.RelatedEntity == "Entity2"),
                                                                                                                              CsdlConstants.DefaultSchemaOrAlias))
                                                              .Returns(csdlNav1);

            // Act
            var actual = CreateEntityAttributeDictionary().GetRelatedEntityMappingProperties(typeof(EntityWithRelatedEntityAlias));

            // Assert
            Assert.AreEqual("E2s", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }
        #endregion

        #region GetRequiredProperty
        [TestMethod]
        public void EntityAttributeDictionary_GetRequiredProperty_PropertyInfo_Null()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            MemberInfo mi = null;

            // Act
            var result = entityAttributeDictionary.GetRequiredProperty(mi);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        
        [TestMethod]
        public void EntityAttributeDictionary_GetRequiredProperty_PropertyInfo_HasRequiredAttribute_Test()
        {
            // Arrange
            var entityAttributeDictionary = CreateEntityAttributeDictionary();
            var mi = typeof(EntityWithRequired).GetProperty(nameof(EntityWithRequired.Name));

            // Act
            var result = entityAttributeDictionary.GetRequiredProperty(mi).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            _MockRepository.VerifyAll();
        }
        #endregion

    }
}
