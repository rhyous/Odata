using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using Rhyous.Odata.Tests.Models;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class PropertyBuilderTests
    {
        [TestMethod]
        public void PropertyBuilder_Build_Null_Test()
        {
            // Arrange
            PropertyInfo propInfo = null;
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyBuilder_Build_Test()
        {
            // Arrange
            var propInfo = typeof(Token).GetProperty("UserId");
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual("Edm.Int64", actual.Type);
            Assert.IsNull(actual.DefaultValue);
            Assert.AreEqual(1, actual.CustomData.Count);
            Assert.AreEqual("$NavigationKey", actual.CustomData.First().Key);
            Assert.AreEqual("User", actual.CustomData.First().Value);
        }

        internal class A { }
        internal class B { public A A { get; set; }}

        [TestMethod]
        public void PropertyBuilder_Build_NotInList_Test()
        {
            // Arrange
            var propInfo = typeof(B).GetProperty("A");
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(actual);
        }

        #region CsdlProperty
        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_DefaultValue_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.DefaultValue));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual(-1, csdl.DefaultValue);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_NotRequired_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.NotRequired));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);
            var isRequired = csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequiredValue) && (bool)isRequiredValue;

            // Assert
            Assert.IsFalse(isRequired);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_Required_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.RequiredDespiteBeingNullable));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsTrue(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequiredValue));
            Assert.IsTrue((bool)isRequiredValue);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_RequiredDefaultSoNoMetadata_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.RequiredNoMetadata));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out _));
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_NotRequiredDefaultSoNoMetadata_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.NotRequiredNoMetadata));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out _));
        }


        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_ForceNullableToNotBeNullable_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.ForceNullableToNotBeNullable));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.Nullable);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_MinLength_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.MinLength));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();
            ulong expected = 5;

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsTrue(csdl.CustomData.TryGetValue(CsdlConstants.UIMinLength, out object uiMinLength));
            Assert.AreEqual(expected, uiMinLength);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_MaxLength_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.MaxLength));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();
            ulong expected = 10;

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual(expected, csdl.MaxLength);
            Assert.IsTrue(csdl.CustomData.TryGetValue(CsdlConstants.UIMaxLength, out object uiMaxLength));
            Assert.AreEqual(expected, uiMaxLength);
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_CustomTypePassword_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.Password));
            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual("Edm.Password", csdl.Type);
        }
        #endregion
    }
}
