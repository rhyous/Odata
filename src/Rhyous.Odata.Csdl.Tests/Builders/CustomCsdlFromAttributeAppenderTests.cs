using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class CustomCsdlFromAttributeAppenderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomPropertyDataFuncs> _MockCustomPropertyDataFuncs;

        private Mock<IEntityAttributeDictionary> _MockEntityAttributeDictionary;
        private Mock<IPropertyAttributeDictionary> _MockPropertyAttributeDictionary;
        private Mock<IPropertyDataAttributeDictionary> _MockPropertyDataAttributeDictionary;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomPropertyDataFuncs = _MockRepository.Create<ICustomPropertyDataFuncs>();
            _MockEntityAttributeDictionary = _MockRepository.Create<IEntityAttributeDictionary>();
            _MockPropertyAttributeDictionary = _MockRepository.Create<IPropertyAttributeDictionary>();
            _MockPropertyDataAttributeDictionary = _MockRepository.Create<IPropertyDataAttributeDictionary>();
        }

        private ICustomCsdlFromAttributeAppender CreateCustomCsdlFromAttributeAppender()
        {
            return new CustomCsdlFromAttributeAppender(
                _MockEntityAttributeDictionary.Object,
                _MockPropertyAttributeDictionary.Object,
                _MockPropertyDataAttributeDictionary.Object);
        }

        #region AddFromAttributes
        [TestMethod]
        public void CustomCsdlFromAttributeAppender_AppendPropertiesFromPropertyAttributes_Valid_Test()
        {
            // Arrange
            var customCsdlFromAttributeAppender = CreateCustomCsdlFromAttributeAppender();
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> func = (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; };
            
            actionDictionary.Add(typeof(RelatedEntityAttribute), func);

            var mockEnumerator = _MockRepository.Create<IEnumerator<KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>>>();
            _MockPropertyAttributeDictionary.Setup(m => m.GetEnumerator()).Returns(actionDictionary.GetEnumerator());

            _MockPropertyAttributeDictionary.Setup(m=>m.TryGetValue(typeof(RelatedEntityAttribute), out func))
                                            .Returns(true);

            // Act
            customCsdlFromAttributeAppender.AppendPropertiesFromPropertyAttributes(dictionary, propInfo);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("1", dictionary.Keys.First());
            Assert.AreEqual("a", dictionary.Values.First());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomCsdlFromAttributeAppender_AppendPropertiesFromPropertyAttributes_NullDictionary_Test()
        {
            // Arrange
            var customCsdlFromAttributeAppender = CreateCustomCsdlFromAttributeAppender();
            Dictionary<string, object> dictionary = null;
            var propInfo = typeof(Token).GetProperty("UserId");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            customCsdlFromAttributeAppender.AppendPropertiesFromPropertyAttributes(dictionary, propInfo);

            // Assert
            Assert.IsNull(dictionary);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomCsdlFromAttributeAppender_AppendPropertiesFromPropertyAttributes_NullMemberInfo_Test()
        {
            // Arrange
            var customCsdlFromAttributeAppender = CreateCustomCsdlFromAttributeAppender();
            var dictionary = new Dictionary<string, object>();
            MemberInfo memberInfo = null;
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            customCsdlFromAttributeAppender.AppendPropertiesFromPropertyAttributes(dictionary, memberInfo);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomCsdlFromAttributeAppender_AppendPropertiesFromPropertyAttributes_PropertyHasNone_Test()
        {
            // Arrange
            var customCsdlFromAttributeAppender = CreateCustomCsdlFromAttributeAppender();
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("Text");

            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> func = (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; };
            actionDictionary.Add(typeof(RelatedEntityAttribute), func);

            var mockEnumerator = _MockRepository.Create<IEnumerator<KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>>>();
            _MockPropertyAttributeDictionary.Setup(m => m.GetEnumerator()).Returns(actionDictionary.GetEnumerator());

            // Act
            customCsdlFromAttributeAppender.AppendPropertiesFromPropertyAttributes(dictionary, propInfo);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
