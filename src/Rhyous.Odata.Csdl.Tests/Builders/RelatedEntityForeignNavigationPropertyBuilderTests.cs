using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class RelatedEntityForeignNavigationPropertyBuilderTests
    {
        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            RelatedEntityForeignAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1");

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
