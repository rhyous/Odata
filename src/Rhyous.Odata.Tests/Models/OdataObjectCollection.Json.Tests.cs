using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Models
{
    [TestClass]
    public class OdataObjectCollectionJsonTests
    {
        [TestMethod]
        public void ImplicitConversionNullTest()
        {
            // Arrange
            OdataObjectCollection obj = null;

            // Act
            RelatedEntityCollection c = obj;

            // Assert
            Assert.IsNull(c);
        }

        [TestMethod]
        public void ImplicitConversionDefaultNewTest()
        {
            // Arrange
            var obj = new OdataObjectCollection();

            // Act
            RelatedEntityCollection c = obj;

            // Assert
            Assert.IsNotNull(c);
            Assert.IsNull(c.Entity);
            Assert.AreEqual(0, c.Count);
        }
    }
}
