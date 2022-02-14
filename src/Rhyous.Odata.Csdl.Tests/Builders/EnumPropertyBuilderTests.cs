using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class EnumPropertyBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomCsdlFromAttributeAppender> _MockCustomCsdlFromAttributeAppenderr;
        private Mock<ICustomPropertyDataAppender> _MockCustomPropertyDataAppender;
        private ICsdlTypeDictionary _CsdlTypeDictionary;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomCsdlFromAttributeAppenderr = _MockRepository.Create<ICustomCsdlFromAttributeAppender>();
            _MockCustomPropertyDataAppender = _MockRepository.Create<ICustomPropertyDataAppender>();
            _CsdlTypeDictionary = new CsdlTypeDictionary();
            _CsdlTypeDictionary.Init();
        }

        private EnumPropertyBuilder CreateEnumPropertyBuilder()
        {
            return new EnumPropertyBuilder(
                _MockCustomCsdlFromAttributeAppenderr.Object,
                _MockCustomPropertyDataAppender.Object,
                _CsdlTypeDictionary);
        }

        #region Build
        [TestMethod]
        public void EnumPropertyBuilder_Build_EnumProperty_Test()
        {
            // Arrange
            var enumPropertyBuilder = CreateEnumPropertyBuilder();
            var type = typeof(EntityWithEnum);
            PropertyInfo propInfo = type.GetProperty(nameof(EntityWithEnum.Type));
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<Dictionary<string, object>>(), type.Name, nameof(EntityWithEnum.Type)));
            _MockCustomCsdlFromAttributeAppenderr.Setup(m => m.AppendPropertiesFromEntityAttributes(It.IsAny<Dictionary<string, object>>(), propInfo));

            // Act
            var result = enumPropertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual(CsdlConstants.EnumType, result.Kind);
            Assert.AreEqual(CsdlConstants.EdmInt32, result.UnderlyingType);
            Assert.AreEqual(2, result.CustomData.Count);
            Assert.AreEqual(CoolType.Awesome, result.CustomData["Awesome"]);
            Assert.AreEqual(CoolType.Cool, result.CustomData["Cool"]);
            Assert.IsFalse(result.IsFlags);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
