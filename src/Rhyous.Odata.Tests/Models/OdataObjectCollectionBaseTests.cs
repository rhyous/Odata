using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Models
{
    [TestClass]
    public class OdataObjectCollectionBaseTests
    {
        [TestMethod]
        public void TotalCountReturnsCountIfLessThenCount()
        {
            // Arrange
            var collection = new OdataObjectCollection { new OdataObject() };

            // Act and Assert
            Assert.AreEqual(1, collection.TotalCount);
        }

        [TestMethod]
        public void TotalCountReturnsTotalCountIfGreaterThenCount()
        {
            // Arrange
            var collection = new OdataObjectCollection { TotalCount = 10 };
            collection.Add(new OdataObject());

            // Act and Assert
            Assert.AreEqual(10, collection.TotalCount);
        }
    }
}
