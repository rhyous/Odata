using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Tests;
using Rhyous.Odata.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class PropertyBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomCsdlFromAttributeAppender> _MockCustomCsdlFromAttributeAppender;
        private Mock<ICustomPropertyDataAppender> _MockCustomPropertyDataAppender;
        private CsdlTypeDictionary _CsdlTypeDictionary;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomCsdlFromAttributeAppender = _MockRepository.Create<ICustomCsdlFromAttributeAppender>();
            _MockCustomPropertyDataAppender = _MockRepository.Create<ICustomPropertyDataAppender>();
            _CsdlTypeDictionary = new CsdlTypeDictionary();
        }

        private PropertyBuilder CreatePropertyBuilder()
        {
            return new PropertyBuilder(
                _MockCustomCsdlFromAttributeAppender.Object,
                _MockCustomPropertyDataAppender.Object,
                _CsdlTypeDictionary);
        }

        [TestMethod]
        public void PropertyBuilder_Build_Null_Test()
        {
            // Arrange
            PropertyInfo propInfo = null;
            var propertyBuilder = CreatePropertyBuilder();

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_Test()
        {
            // Arrange
            var propInfo = typeof(Token).GetProperty(nameof(Token.UserId));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(Token).Name, nameof(Token.UserId)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual("Edm.Int64", actual.Type);
            Assert.AreEqual(0, actual.CustomData.Count);
            _MockRepository.VerifyAll();
        }

        internal class A { }
        internal class B { public A A { get; set; }}

        [TestMethod]
        public void PropertyBuilder_Build_NotInList_Test()
        {
            // Arrange
            var propInfo = typeof(B).GetProperty(nameof(B.A));
            var propertyBuilder = CreatePropertyBuilder();

            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        #region CsdlProperty
        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_DefaultValue_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.DefaultValue));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.DefaultValue)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual(-1, csdl.DefaultValue);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_NotRequired_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.NotRequired));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.NotRequired)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));


            // Act
            var csdl = propertyBuilder.Build(propInfo);
            var isRequired = csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequiredValue) && (bool)isRequiredValue;

            // Assert
            Assert.IsFalse(isRequired);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_Required_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.RequiredDespiteBeingNullable));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.RequiredDespiteBeingNullable)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)))
                                  .Callback((IDictionary<string, object> propertyDictionary, MemberInfo mi) => 
                                  {
                                      propertyDictionary.Add(CsdlConstants.UIRequired, true);
                                  });

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsTrue(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequiredValue));
            Assert.IsTrue((bool)isRequiredValue);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_RequiredDefaultSoNoMetadata_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.RequiredNoMetadata));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.RequiredNoMetadata)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out _));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_NotRequiredDefaultSoNoMetadata_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.NotRequiredNoMetadata));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.NotRequiredNoMetadata)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.CustomData.TryGetValue(CsdlConstants.UIRequired, out _));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_ForceNullableToNotBeNullable_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.ForceNullableToNotBeNullable));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.ForceNullableToNotBeNullable)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsFalse(csdl.Nullable);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PropertyBuilder_Build_PropertyIsByteArray_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithByteArray).GetProperty(nameof(EntityWithByteArray.Data));
            var propertyBuilder = CreatePropertyBuilder();

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(csdl);
            _MockRepository.VerifyAll();
        }

        #endregion
    }
}
