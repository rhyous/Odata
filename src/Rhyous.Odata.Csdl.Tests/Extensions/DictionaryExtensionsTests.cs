using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        #region AddIfNewAndNotNull
        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_KeyNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = null;
            string value = "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_KeyEmpty_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "";
            string value = "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        public void DictionaryExtensions_AddIfNewAndNotNull_KeyWhitespace_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "";
            string value = "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_ValueNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "some key";
            string value = null;

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_KeyAndValueSet_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "some key";
            string value = "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_Duplicate_Test()
        {
            // Arrange
            string key = "some key";
            string value = "some value";
            var dictionary = new Dictionary<string, object> { { key, value } };

            // Act
            dictionary.AddIfNewAndNotNull(key, value);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
        }
        #endregion
        
        #region AddIfNewAndNotNull Func
        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_func_KeyNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = null;
            Func<object> func = () => "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_func_KeyEmpty_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "";
            Func<object> func = () => "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        public void DictionaryExtensions_AddIfNewAndNotNull_func_KeyWhitespace_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "";
            Func<object> func = () => "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_func_ValueNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "some key";
            Func<object> func = () => null;

            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_func_KeyAndValueSet_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            string key = "some key";
            Func<object> func = () => "some value";

            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddIfNewAndNotNull_func_Duplicate_Test()
        {
            // Arrange
            string key = "some key";
            string value = "some value";
            Func<object> func = () => value;
            var dictionary = new Dictionary<string, object> { { key, value } };
            
            // Act
            dictionary.AddIfNewAndNotNull(key, func);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
        }
        #endregion

        #region AddFromAttributes
        [TestMethod]
        public void DictionaryExtensions_AddFromAttributes_Valid_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            dictionary.AddFromAttributes(propInfo, actionDictionary);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("1", dictionary.Keys.First());
            Assert.AreEqual("a", dictionary.Values.First());
        }

        [TestMethod]
        public void DictionaryExtensions_AddFromAttributes_NullDictionary_Test()
        {
            // Arrange
            Dictionary<string, object> dictionary = null;
            var propInfo = typeof(Token).GetProperty("UserId");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            dictionary.AddFromAttributes(propInfo, actionDictionary);

            // Assert
            Assert.IsNull(dictionary);
        }

        [TestMethod]
        public void DictionaryExtensions_AddFromAttributes_NullMemberInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            MemberInfo memberInfo = null;
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            dictionary.AddFromAttributes(memberInfo, actionDictionary);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddFromAttributes_NullActionDictionary_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");
            Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> actionDictionary = null;
            
            // Act
            dictionary.AddFromAttributes(propInfo, actionDictionary);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }

        [TestMethod]
        public void DictionaryExtensions_AddFromAttributes_PropertyHasNone_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("Text");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            dictionary.AddFromAttributes(propInfo, actionDictionary);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }
        #endregion

        #region AddFromPropertyInfo
        [TestMethod]
        public void DictionaryExtensions_AddFromPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");

            // Act
            dictionary.AddFromPropertyInfo(propInfo);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("UserId", dictionary.Keys.First());
            Assert.AreEqual(typeof(CsdlProperty), dictionary.Values.First().GetType());

        }

        [TestMethod]
        public void DictionaryExtensions_AddFromPropertyInfo_NullDictionary_Test()
        {
            // Arrange
            Dictionary<string, object> dictionary = null;
            var propInfo = typeof(Token).GetProperty("UserId");
            var actionDictionary = new Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();
            actionDictionary.Add(typeof(RelatedEntityAttribute), (MemberInfo mi) => { return new[] { new KeyValuePair<string, object>("1", "a") }; });

            // Act
            dictionary.AddFromAttributes(propInfo, actionDictionary);

            // Assert
            Assert.IsNull(dictionary);
        }

        [TestMethod]
        public void DictionaryExtensions_AddFromPropertyInfo_NullPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            PropertyInfo propInfo = null;

            // Act
            dictionary.AddFromPropertyInfo(propInfo);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
        }
        #endregion

        #region AddCustomProperties
        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_DictionaryNull_Test()
        {
            // Arrange
            Type type = typeof(User);
            bool func1WasCalled = false;
            IEnumerable<KeyValuePair<string, object>> func1(Type t)
            {
                func1WasCalled = true;
                return null;
            }
            Dictionary<string, object> dictionary = null;

            // Act
            dictionary.AddCustomProperties(type, func1);

            // Assert
            Assert.IsNull(dictionary);
            Assert.IsFalse(func1WasCalled);
        }

        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_TypeNull_Test()
        {
            // Arrange
            Type type = null;
            bool func1WasCalled = false;
            IEnumerable<KeyValuePair<string, object>> func1(Type t)
            {
                func1WasCalled = true;
                return null;
            }
            var dictionary = new Dictionary<string, object>();

            // Act
            dictionary.AddCustomProperties(type, func1);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
        }

        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_FuncNull_Test()
        {
            // Arrange
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>>[] funcs = null;
            var dictionary = new Dictionary<string, object>();

            // Act
            dictionary.AddCustomProperties(type, funcs);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
        }

        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_FuncsEmpty_Test()
        {
            // Arrange
            Type type = typeof(User);
            bool func1WasCalled = false;
            var funcs = new Func<Type, IEnumerable<KeyValuePair<string, object>>>[0];
            var dictionary = new Dictionary<string, object>();

            // Act
            dictionary.AddCustomProperties(type, funcs);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
        }

        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_FuncsItemNull_Test()
        {
            // Arrange
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>> func1 = null;
            var dictionary = new Dictionary<string, object>();

            // Act
            dictionary.AddCustomProperties(type, func1);

            // Assert
            Assert.AreEqual(0, dictionary.Count);
            Assert.IsFalse(func1WasCalled);
        }

        [TestMethod]
        public void DictionaryExtensions_AddCustomProperties_Valid_Test()
        {
            // Arrange
            Type type = typeof(User);
            bool func1WasCalled = false;
            Func<Type, IEnumerable<KeyValuePair<string, object>>> func1 = (Type t) => 
            {
                func1WasCalled = true;
                return new[] { new KeyValuePair<string, object>("a", "b") };
            };
            var dictionary = new Dictionary<string, object>();

            // Act
            dictionary.AddCustomProperties(type, func1);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(func1WasCalled);
        }
        #endregion
    }
}
