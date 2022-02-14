using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Tests;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class PropertyAttributeDictionaryTests
    {
        private MockRepository _MockRepository;

        private Mock<IRelatedEntityNavigationPropertyBuilder> _MockRelatedEntityNavigationPropertyBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityNavigationPropertyBuilder = _MockRepository.Create<IRelatedEntityNavigationPropertyBuilder>();
        }

        private PropertyAttributeDictionary CreatePropertyAttributeDictionary()
        {
            return new PropertyAttributeDictionary(
                _MockRelatedEntityNavigationPropertyBuilder.Object);
        }
        #region GetRelatedEntityProperties

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_Null_Test()
        {
            // Arrange
            var propertyAttributeDictionary = CreatePropertyAttributeDictionary();
            // Act
            var actual = propertyAttributeDictionary.GetRelatedEntityProperties(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_NoAttributes_Test()
        {
            // Arrange
            var propertyAttributeDictionary = CreatePropertyAttributeDictionary();
            // Act
            var actual = propertyAttributeDictionary.GetRelatedEntityProperties(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_AttributeExists_Test()
        {
            // Arrange
            var propertyAttributeDictionary = CreatePropertyAttributeDictionary();
            var csdlNav = new CsdlNavigationProperty();
            _MockRelatedEntityNavigationPropertyBuilder.Setup(m=>m.Build(It.Is<RelatedEntityAttribute>(rea => rea.RelatedEntity == nameof(UserType)),
                                                                                CsdlConstants.DefaultSchemaOrAlias))
                                                       .Returns(csdlNav);

            // Act
            var actual = propertyAttributeDictionary.GetRelatedEntityProperties(typeof(User).GetProperty("UserTypeId"));

            // Assert
            Assert.AreEqual("UserType", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_AttributeExistsWithAlias_Test()
        {
            // Arrange
            var propertyAttributeDictionary = CreatePropertyAttributeDictionary();
            var csdlNav = new CsdlNavigationProperty();
            _MockRelatedEntityNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityAttribute>(rea => rea.RelatedEntity == nameof(Entity3)),
                                                                                CsdlConstants.DefaultSchemaOrAlias))
                                                       .Returns(csdlNav);

            // Act
            var actual = propertyAttributeDictionary.GetRelatedEntityProperties(typeof(EntityWithRelatedEntityAlias).GetProperty("Entity3Id"));

            // Assert
            Assert.AreEqual("E3", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_TwoAttributesExists_OneWithAlias_Test()
        {
            // Arrange
            var propertyAttributeDictionary = CreatePropertyAttributeDictionary();
            var csdlNav = new CsdlNavigationProperty();
            _MockRelatedEntityNavigationPropertyBuilder.Setup(m => m.Build(It.Is<RelatedEntityAttribute>(rea => rea.RelatedEntity == nameof(Entity3)),
                                                                                CsdlConstants.DefaultSchemaOrAlias))
                                                       .Returns(csdlNav);

            // Act
            var actual = propertyAttributeDictionary.GetRelatedEntityProperties(typeof(EntityWithDuplicateRelatedEntityOneAlias).GetProperty("Entity3Id"));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("E3", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.FirstOrDefault().Value.GetType());
        }
        #endregion
    }
}
