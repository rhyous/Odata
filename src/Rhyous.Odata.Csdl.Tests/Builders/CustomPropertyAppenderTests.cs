using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class CustomPropertyAppenderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomPropertyFuncs> _MockCustomPropertyFuncs;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomPropertyFuncs = _MockRepository.Create<ICustomPropertyFuncs>();
        }

        private CustomPropertyAppender CreateCustomPropertyAppender()
        {
            return new CustomPropertyAppender(
                _MockCustomPropertyFuncs.Object);
        }

        #region AddCustomProperties
        #endregion

        #region AddCustomProperties Generic
        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_DictionaryNull_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            IConcurrentDictionary<string, object> dictionary = null;
            Type type = typeof(User);
            bool func1WasCalled = false;
            IEnumerable<KeyValuePair<string, object>> func1(Type t)
            {
                func1WasCalled = true;
                return null;
            }

            // Act
            customCsdlBuilder.Append(dictionary, type, func1);

            // Assert
            Assert.IsNull(dictionary);
            Assert.IsFalse(func1WasCalled);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_TypeNull_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            var dictionary = new ConcurrentDictionaryWrapper<string, object>();
            Type type = null;
            bool func1WasCalled = false;
            IEnumerable<KeyValuePair<string, object>> func1(Type t)
            {
                func1WasCalled = true;
                return null;
            }

            // Act
            customCsdlBuilder.Append(dictionary, type, func1);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_FuncNull_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            var dictionary = new ConcurrentDictionaryWrapper<string, object>();
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>>[] funcs = null;

            // Act
            customCsdlBuilder.Append(dictionary, type, funcs);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_FuncsEmpty_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            var dictionary = new ConcurrentDictionaryWrapper<string, object>();
            Type type = typeof(User);
            bool func1WasCalled = false;
            var funcs = new Func<Type, IEnumerable<KeyValuePair<string, object>>>[0];

            // Act
            customCsdlBuilder.Append(dictionary, type, funcs);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_FuncsItemNull_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            var dictionary = new ConcurrentDictionaryWrapper<string, object>();
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>> func1 = null;

            // Act
            customCsdlBuilder.Append(dictionary, type, func1);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyAppender_AddCustomProperties_Valid_Test()
        {
            // Arrange
            var customCsdlBuilder = CreateCustomPropertyAppender();
            var dictionary = new SortedConcurrentDictionary<string, object>();
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>> func1 = (Type t) =>
            {
                func1WasCalled = true;
                return new[] { new KeyValuePair<string, object>("a", "b") };
            };

            // Act
            customCsdlBuilder.Append(dictionary, type, func1);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(func1WasCalled);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
