using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Tests;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class PropertyBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomCsdlFromAttributeAppender> _MockCustomCsdlFromAttributeAppender;
        private Mock<ICustomPropertyDataAppender> _MockCustomPropertyDataAppender;
        private Mock<ICsdlTypeDictionary> _MockCsdlTypeDictionary;
        private Mock<IMinLengthAttributeDictionary> _MockMinLengthAttributeDictionary;
        private Mock<IMaxLengthAttributeDictionary> _MockMaxLengthAttributeDictionary;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomCsdlFromAttributeAppender = _MockRepository.Create<ICustomCsdlFromAttributeAppender>();
            _MockCustomPropertyDataAppender = _MockRepository.Create<ICustomPropertyDataAppender>();
            _MockCsdlTypeDictionary = _MockRepository.Create<ICsdlTypeDictionary>();
            _MockMinLengthAttributeDictionary = _MockRepository.Create<IMinLengthAttributeDictionary>();
            _MockMaxLengthAttributeDictionary = _MockRepository.Create<IMaxLengthAttributeDictionary>();
        }

        private PropertyBuilder CreatePropertyBuilder()
        {
            return new PropertyBuilder(
                _MockCustomCsdlFromAttributeAppender.Object,
                _MockCustomPropertyDataAppender.Object,
                _MockCsdlTypeDictionary.Object,
                _MockMinLengthAttributeDictionary.Object,
                _MockMaxLengthAttributeDictionary.Object);
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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(long).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int64";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)1);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)20);
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
        public void PropertyBuilder_Build_ComplexType_NotInList_Test()
        {
            // Arrange
            var propInfo = typeof(B).GetProperty(nameof(B.A));
            var propertyBuilder = CreatePropertyBuilder();
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(A).FullName, out edmType))
                       .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                       {
                           outEdmType = null;
                           return false;
                       }));

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) => 
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual(-1, csdl.DefaultValue);
            _MockRepository.VerifyAll();
        } delegate bool TryGetValueDelegate(string instr, out string outStr);

        [TestMethod]
        public void PropertyBuilder_Build_CsdlAttribute_NotRequired_Test()
        {
            // Arrange
            var propInfo = typeof(EntityWithCsdlPropertyAttribute).GetProperty(nameof(EntityWithCsdlPropertyAttribute.NotRequired));
            var propertyBuilder = CreatePropertyBuilder();
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), typeof(EntityWithCsdlPropertyAttribute).Name, nameof(EntityWithCsdlPropertyAttribute.NotRequired)));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(int).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Int32";
                                       return true;
                                   }));
            _MockMinLengthAttributeDictionary.Setup(m => m.GetMinLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)4);
            _MockMaxLengthAttributeDictionary.Setup(m => m.GetMaxLength(It.IsAny<PropertyInfo>()))
                                             .Returns((ulong)100);

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
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(byte[]).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = null;
                                       return false;
                                   }));

            // Act
            var csdl = propertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNull(csdl);
            _MockRepository.VerifyAll();
        }

        #endregion
    }
}
