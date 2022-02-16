using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlSchemaTests
    {
        [TestMethod]
        public void CsdlSchema_LazyGetEntities_Test()
        {
            // Arrange
            var doc = new CsdlSchema();

            // Act
            // Assert
            Assert.IsNotNull(doc.Entities);
        }

        [TestMethod]
        public void CsdlSchema_SetEntities_Test()
        {
            // Arrange
            var doc = new CsdlSchema();
            var expectedSchemas = new SortedConcurrentDictionary<string, object>();

            // Act
            doc.Entities = expectedSchemas;

            // Assert
            Assert.AreEqual(expectedSchemas, doc.Entities);
        }
    }
}
