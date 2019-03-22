using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class PropertyBuilderTests
    {
        [TestMethod]
        public void PropertyBuilder_AddFromPropertyInfo_Test()
        {
            // Arrange
            var dictionary = new Dictionary<string, object>();
            var propInfo = typeof(Token).GetProperty("UserId");

            var propertyBuilder = new CsdlBuilderFactory().CreatePropertyBuilder();
            // Act
            var actual = propertyBuilder.Build(propInfo);

            // Assert
            Assert.AreEqual("Edm.Int64", actual.Type);
            Assert.IsNull(actual.DefaultValue);
            Assert.AreEqual(1, actual.CustomData.Count);
            Assert.AreEqual("$NavigationKey", actual.CustomData.First().Key);
            Assert.AreEqual("User", actual.CustomData.First().Value);
        }
    }
}
