using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;

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
            var expectedProps = new SortedConcurrentDictionary<string, object>();

            // Act
            doc.Properties = expectedProps;

            // Assert
            Assert.AreEqual(expectedProps, doc.Properties);
        }

        [TestMethod]
        public void CsdlEntity_Properties_SortedCorrectly_Test()
        {
            // Arrange
            var doc = new CsdlEntity();
            doc.Properties.Add("@CustomProperty1", new CsdlProperty());
            doc.Properties.Add("Id", new CsdlProperty());
            doc.Properties.Add("Name", new CsdlProperty());
            doc.Properties.Add("$Kind", new CsdlProperty());

            // Act
            var list = doc.Properties.ToList();

            // Assert
            Assert.AreEqual("$Kind", list[0].Key);
            Assert.AreEqual("Id", list[1].Key);
            Assert.AreEqual("Name", list[2].Key);
            Assert.AreEqual("@CustomProperty1", list[3].Key);
        }
    }
}
