using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class EntityBuilderTests
    {
        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_DictionaryNull_Test()
        {
            // Arrange
            Dictionary<string, object> dictionary = null;
            PropertyInfo propInfo = null;
            var entityBuilder = new CsdlBuilderFactory().CreateEntityBuilder();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entityBuilder.AddFromPropertyInfo(dictionary, propInfo);
            });
        }

        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_PropInfoNull_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            PropertyInfo propInfo = null;
            var entityBuilder = new CsdlBuilderFactory().CreateEntityBuilder();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                entityBuilder.AddFromPropertyInfo(dictionary, propInfo);
            });
        }

        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");
            var entityBuilder = new CsdlBuilderFactory().CreateEntityBuilder();

            // Act
            entityBuilder.AddFromPropertyInfo(dictionary, propInfo);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("UserId", dictionary.Keys.First());
            Assert.AreEqual(typeof(CsdlProperty), dictionary.Values.First().GetType());
        }

        [TestMethod]
        public void EntityBuilder_Build_ExcludeFromMetadata_Test()
        {
            // Arrange
            var entityBuilder = new CsdlBuilderFactory().CreateEntityBuilder();

            // Act
            var actual = entityBuilder.Build(typeof(EntityExcludeFromMetadata));

            // Assert
            Assert.AreEqual(2, actual.Properties.Count);
            Assert.AreEqual("Id", actual.Properties.Keys.First());
            Assert.AreEqual("Name", actual.Properties.Keys.Skip(1).First());
        }
    }
}
