﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlDocumentTests
    {
        [TestMethod]
        public void CsdlDocument_LazyGetSchemas_Test()
        {
            // Arrange
            var doc = new CsdlDocument();

            // Act
            // Assert
            Assert.IsNotNull(doc.Schemas);
        }

        [TestMethod]
        public void CsdlDocument_SetSchemas_Test()
        {
            // Arrange
            var doc = new CsdlDocument();
            var expectedSchemas = new SortedConcurrentDictionary<string, object>();

            // Act
            doc.Schemas = expectedSchemas;

            // Assert
            Assert.AreEqual(expectedSchemas, doc.Schemas);
        }
    }
}
