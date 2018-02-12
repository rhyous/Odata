using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Models
{
    [TestClass]
    public class OdataObjectCollectionTests
    {
        [TestMethod]
        public void ImplicitConversionNullTest()
        {
            // Arrange
            OdataObjectCollection<User, int> obj = null;

            // Act
            RelatedEntityCollection c = obj;

            // Assert
            Assert.IsNull(c);
        }

        [TestMethod]
        public void ImplicitConversionDefaultNewTest()
        {
            // Arrange
            var obj = new OdataObjectCollection<User, int>();

            // Act
            RelatedEntityCollection c = obj;

            // Assert
            Assert.IsNotNull(c);
            Assert.IsNull(c.Entity);
            Assert.AreEqual(0, c.Count);
        }
    }
}
