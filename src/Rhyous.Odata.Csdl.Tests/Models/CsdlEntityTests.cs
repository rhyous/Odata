using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlEntityTests
    {
        [TestMethod]
        public void CsdlEntity_LazyGetKeys_Test()
        {
            // Arrange
            var e = new CsdlEntity();
            // Act
            // Assert
            Assert.IsNotNull(e.Keys);
        }

        [TestMethod]
        public void CsdlEntity_SetKeys_Test()
        {
            // Arrange
            var doc = new CsdlEntity();
            var expectedKeys = new System.Collections.Generic.List<string>();

            // Act
            doc.Keys = expectedKeys;

            // Assert
            Assert.AreEqual(expectedKeys, doc.Keys);
        }

        [TestMethod]
        public void CsdlEntity_LazyGetProperties_Test()
        {
            // Arrange
            var e = new CsdlEntity();
            // Act
            // Assert
            Assert.IsNotNull(e.Properties);
        }

        [TestMethod]
        public void CsdlEntity_SetProperties_Test()
        {
            // Arrange
            var doc = new CsdlEntity();
            var expectedProps = new System.Collections.Generic.Dictionary<string, object>();

            // Act
            doc.Properties = expectedProps;

            // Assert
            Assert.AreEqual(expectedProps, doc.Properties);
        }
    }
}
