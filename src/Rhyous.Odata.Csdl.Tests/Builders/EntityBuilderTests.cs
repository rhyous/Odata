using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class EntityBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<IPropertyBuilder> _MockPropertyBuilder;
        private Mock<IEnumPropertyBuilder> _MockEnumPropertyBuilder;
        private Mock<ICustomCsdlFromAttributeAppender> _MockCustomCsdlFromAttributeAppender;
        private Mock<ICustomPropertyAppender> _MockCustomPropertyAppender;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockPropertyBuilder = _MockRepository.Create<IPropertyBuilder>();
            _MockEnumPropertyBuilder = _MockRepository.Create<IEnumPropertyBuilder>();
            _MockCustomCsdlFromAttributeAppender = _MockRepository.Create<ICustomCsdlFromAttributeAppender>();
            _MockCustomPropertyAppender = _MockRepository.Create<ICustomPropertyAppender>();
        }

        private EntityBuilder CreateEntityBuilder()
        {
            return new EntityBuilder(
                _MockPropertyBuilder.Object,
                _MockEnumPropertyBuilder.Object,
                _MockCustomCsdlFromAttributeAppender.Object,
                _MockCustomPropertyAppender.Object);
        }

        #region AddFromPropertyInfo
        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_DictionaryNull_Test()
        {
            // Arrange
            Dictionary<string, object> dictionary = null;
            PropertyInfo propInfo = null;
            var entityBuilder = CreateEntityBuilder();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entityBuilder.AddFromPropertyInfo(dictionary, propInfo);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_PropInfoNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            PropertyInfo propInfo = null;
            var entityBuilder = CreateEntityBuilder();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entityBuilder.AddFromPropertyInfo(dictionary, propInfo);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");
            var entityBuilder = CreateEntityBuilder();
            var csdlProperty = new CsdlProperty();
            _MockPropertyBuilder.Setup(m=>m.Build(propInfo))
                                .Returns(csdlProperty);

            // Act
            entityBuilder.AddFromPropertyInfo(dictionary, propInfo);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("UserId", dictionary.Keys.First());
            Assert.AreEqual(typeof(CsdlProperty), dictionary.Values.First().GetType());
            _MockRepository.VerifyAll();
        }

        #endregion

        #region Build
        [TestMethod]
        public void EntityBuilder_Build_ExcludeFromMetadata_Test()
        {
            // Arrange
            var entityBuilder = CreateEntityBuilder();
            var type = typeof(EntityExcludeFromMetadata);

            var csdlProperties = new List<CsdlProperty>();

            var propInfos = type.GetProperties().OrderBy(p => p.Name);
            foreach (var propInfo in propInfos)
            {
                if (propInfo.ExcludeFromMetadata())
                    continue;
                var csdlProperty = new CsdlProperty();
                csdlProperties.Add(csdlProperty);
                _MockPropertyBuilder.Setup(m => m.Build(It.Is<PropertyInfo>(pi=> pi.Name == propInfo.Name)))
                                    .Returns(csdlProperty);
                _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertiesFromPropertyAttributes(It.IsAny<Dictionary<string, object>>(), propInfo));
            }
            _MockCustomPropertyAppender.Setup(m => m.Append(It.IsAny<Dictionary<string, object>>(), type.Name));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertiesFromEntityAttributes(It.IsAny<Dictionary<string, object>>(), type));

            // Act 
            var actual = entityBuilder.Build(type);

            // Assert
            Assert.AreEqual(2, actual.Properties.Count);
            Assert.AreEqual("Id", actual.Properties.Keys.First());
            Assert.AreEqual("Name", actual.Properties.Keys.Skip(1).First());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityBuilder_CreateCsdl_Test()
        {
            // Arrange
            var entityBuilder = CreateEntityBuilder();            
            var type = typeof(User);

            var csdlProperties = new List<CsdlProperty>();

            var propInfos = type.GetProperties().OrderBy(p => p.Name);
            foreach (var propInfo in propInfos)
            {
                var csdlProperty = new CsdlProperty();
                csdlProperties.Add(csdlProperty);
                _MockPropertyBuilder.Setup(m => m.Build(It.Is<PropertyInfo>(pi => pi.Name == propInfo.Name)))
                                    .Returns(csdlProperty);
                _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertiesFromPropertyAttributes(It.IsAny<Dictionary<string, object>>(), propInfo));
            }
            _MockCustomPropertyAppender.Setup(m => m.Append(It.IsAny<Dictionary<string, object>>(), type.Name));
            _MockCustomCsdlFromAttributeAppender.Setup(m => m.AppendPropertiesFromEntityAttributes(It.IsAny<Dictionary<string, object>>(), type));


            // Act
            var actual = entityBuilder.Build(typeof(User));

            // Assert
            Assert.AreEqual(3, actual.Properties.Count);
            Assert.AreEqual("Id", actual.Properties.Keys.First());
            Assert.AreEqual("Name", actual.Properties.Keys.Second());
            Assert.AreEqual("UserTypeId", actual.Properties.Keys.Third());
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
