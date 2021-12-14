using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class RelatedEntityNavigationPropertyBuilderTests
    {
        [TestMethod]
        public void RelatedEntityNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityNavigationPropertyBuilder(funcs);
            RelatedEntityAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RelatedEntityNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityAttribute("Entity2");

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            Assert.IsFalse(result.IsCollection);
            Assert.IsFalse(result.Nullable);
        }


        [TestMethod]
        public void RelatedEntityNavigationPropertyBuilder_Build_ValidAttribute_RelatedEntityMustExistFalse_NullableTrue_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityAttribute("Entity2")
            {
                RelatedEntityMustExist = false,
                Nullable = true
            };

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            Assert.IsFalse(result.IsCollection);
            Assert.IsTrue(result.Nullable);
        }

        [TestMethod]
        public void RelatedEntityNavigationPropertyBuilder_Build_ValidAttribute_AllowNonExistantValue_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityAttribute("Entity2")
            {
                AllowedNonExistentValue = 0
            };

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            var defaultValue = result.CustomData[CsdlConstants.Default] as CsdlNameValue;
            Assert.AreEqual(relatedEntityAttribute.AllowedNonExistentValue, defaultValue.Value);
            Assert.AreEqual(relatedEntityAttribute.AllowedNonExistentValueName, defaultValue.Name);
            Assert.IsFalse(result.IsCollection);
            Assert.IsFalse(result.Nullable);
        }

        [TestMethod]
        public void RelatedEntityNavigationPropertyBuilder_Build_ValidAttribute_AllowNonExistantValue_Renamed_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityAttribute("Entity2")
            {
                AllowedNonExistentValue = 0,
                AllowedNonExistentValueName = "All"
            };

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            var defaultValue = result.CustomData[CsdlConstants.Default] as CsdlNameValue;
            Assert.AreEqual(relatedEntityAttribute.AllowedNonExistentValue, defaultValue.Value);
            Assert.AreEqual(relatedEntityAttribute.AllowedNonExistentValueName, defaultValue.Name);
            Assert.IsFalse(result.IsCollection);
            Assert.IsFalse(result.Nullable);
        }
    }
}