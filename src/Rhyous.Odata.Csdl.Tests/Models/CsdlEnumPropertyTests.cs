using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlEnumPropertyTests
    {
        [TestMethod]
        public void CsdlEnumProperty_LazyGetCustomData_Test()
        {
            // Arrange
            var prop = new CsdlEnumProperty();

            // Act
            // Assert
            Assert.IsNotNull(prop.CustomData);
        }

        [TestMethod]
        public void CsdlEnumProperty_SetCustomData_Test()
        {
            // Arrange
            var prop = new CsdlEnumProperty();
            var expectedCustomData = new System.Collections.Generic.Dictionary<string, object>();
            prop.CustomData = expectedCustomData;

            // Act
            // Assert
            Assert.AreEqual(expectedCustomData, prop.CustomData);
        }
    }
}
