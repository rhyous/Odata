using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class EntityBuilderTests
    {
        [TestMethod]
        public void EntityBuilder_AddFromPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");

            var propertyBuilder = new PropertyBuilder(new PropertyDataAttributeDictionary(), new CustomPropertyDataDictionary());
            var entityBuilder = new EntityBuilder(propertyBuilder, new EnumPropertyBuilder(), new EntityAttributeDictionary(), new PropertyAttributeDictionary());

            // Act
            entityBuilder.AddFromPropertyInfo(dictionary, propInfo);

            // Assert
            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("UserId", dictionary.Keys.First());
            Assert.AreEqual(typeof(CsdlProperty), dictionary.Values.First().GetType());

        }
    }
}
