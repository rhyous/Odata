using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Tests;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class ArrayPropertyBuilderTests
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

        private ArrayPropertyBuilder CreateArrayPropertyBuilder()
        {
            return new ArrayPropertyBuilder(
                _MockCustomCsdlFromAttributeAppender.Object,
                _MockCustomPropertyDataAppender.Object,
                _MockCsdlTypeDictionary.Object);
        }

        delegate bool TryGetValueDelegate(string instr, out string outStr);

        #region CsdlProperty
        [TestMethod]
        public void ArrayPropertyBuilder_Build_PropertyIsByteArray_Test()
        {
            // Arrange
            var type = typeof(EntityWithByteArray);
            var propInfo = type.GetProperty(nameof(EntityWithByteArray.Data));
            var arrayPropertyBuilder = CreateArrayPropertyBuilder();
            string edmType;
            _MockCsdlTypeDictionary.Setup(m => m.TryGetValue(typeof(byte).FullName, out edmType))
                                   .Returns(new TryGetValueDelegate((string inName, out string outEdmType) =>
                                   {
                                       outEdmType = "Edm.Byte";
                                       return true;
                                   }));
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), type.Name, propInfo.Name));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertyDataFromAttributes(It.IsAny<IConcurrentDictionary<string, object>>(), It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)));

            // Act
            var csdl = arrayPropertyBuilder.Build(propInfo);

            // Assert
            Assert.IsNotNull(csdl);
            Assert.IsNotNull("Collection(byte)", csdl.Type);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
