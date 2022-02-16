using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class CustomPropertyDataAppenderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomPropertyDataFuncs> _MockCustomPropertyDataFuncs;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomPropertyDataFuncs = _MockRepository.Create<ICustomPropertyDataFuncs>();
        }

        private CustomPropertyDataAppender CreateCustomPropertyDataAppender(ICustomPropertyDataFuncs customPropertyDataFuncs = null)
        {
            return new CustomPropertyDataAppender(customPropertyDataFuncs ?? _MockCustomPropertyDataFuncs.Object);
        }
        
        #region AddCustomPropertyData
        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_DictionaryNull_Test()
        {
            // Arrange
            var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            SortedConcurrentDictionary<string, object> dictionary = null;
            var entity = "Entity1";
            var prop = "Prop1";
            IFuncList<string, string> funcs = new FuncList<string, string>()
            {
                (e, p) => { return null; }
            };

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.IsNull(dictionary);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_FuncsEmpty_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>(); var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            var entity = "Entity1";
            var prop = "Prop1";
            ICustomPropertyDataFuncs funcs = new CustomPropertyDataFuncs();
            var mockEnumerator = _MockRepository.Create<IEnumerator<KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>>>();
            _MockCustomPropertyDataFuncs.Setup(m => m.GetEnumerator()).Returns(funcs.GetEnumerator());

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_PropNull_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>();
            var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            var entity = "Entity1";
            string prop = null;
            IFuncList<string, string> funcs = new FuncList<string, string>()
            {
                (e, p) => { return null; }
            };

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_PropEmpty_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>();
            var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            var entity = "Entity1";
            var prop = "";
            IFuncList<string, string> funcs = new FuncList<string, string>()
            {
                (e, p) => { return null; }
            };

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_EntityNull_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>(); var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            string entity = null;
            var prop = "prop1";
            IFuncList<string, string> funcs = new FuncList<string, string>()
            {
                (e, p) => { return null; }
            };

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_EntityEmpty_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>(); var customPropertyDataAppender = CreateCustomPropertyDataAppender();
            var entity = "";
            var prop = "prop1";
            IFuncList<string, string> funcs = new FuncList<string, string>()
            {
                (e, p) => { return null; }
            };

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CustomPropertyDataAppender_AddFromCustomDictionary_Valid_Test()
        {
            // Arrange
            var dictionary = new SortedConcurrentDictionary<string, object>();
            
            var entity = "Entity1";
            var prop = "prop1";
            ICustomPropertyDataFuncs funcs = new CustomPropertyDataFuncs()
            {
                (e, p) => { return new [] { new KeyValuePair<string,object>("1", "a")}; }
            };
            var customPropertyDataAppender = CreateCustomPropertyDataAppender(funcs); 

            // Act
            customPropertyDataAppender.Append(dictionary, entity, prop);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("1", dictionary.Keys.First());
            Assert.AreEqual("a", dictionary.Values.First());
            _MockRepository.VerifyAll();
        }
        #endregion


    }
}
