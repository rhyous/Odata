using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]    
    public class RelatedEntityMappingNavigationPropertyBuilderTests
    {
        [TestMethod]
        public void RelatedEntityMappingNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var mockFuncEnumerable = new Mock<IFuncList<string, string>>();
            var unitUnderTest = new RelatedEntityMappingNavigationPropertyBuilder(mockFuncEnumerable.Object);
            RelatedEntityMappingAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RelatedEntityMappingNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var mockFuncEnumerable = new Mock<IFuncList<string, string>>();
            var unitUnderTest = new RelatedEntityMappingNavigationPropertyBuilder(mockFuncEnumerable.Object);
            var relatedEntityAttribute = new RelatedEntityMappingAttribute("Entity2", "Entity1To2Map", "Entity1");
            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual("NavigationProperty", result.Kind);
            Assert.IsTrue(result.IsCollection);
            Assert.IsTrue(result.Nullable);
        }
    }
}
