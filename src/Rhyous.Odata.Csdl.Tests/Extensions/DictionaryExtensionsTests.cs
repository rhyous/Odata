﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests
{
    [TestClass]
    public class DictionaryExtensionsTests
    { 
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

            var propertyBuilder = new PropertyBuilder(new PropertyDataAttributeDictionary(), new CustomPropertyDataDictionary());
            var entityBuilder = new EntityBuilder(propertyBuilder, new EnumPropertyBuilder(), new EntityAttributeDictionary(), new PropertyAttributeDictionary());

            // Act
            entityBuilder.AddFromPropertyInfo(dictionary, propInfo);

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
