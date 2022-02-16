using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlPropertyTests
    {
        [TestMethod]
        public void CsdlProperty_LazyGetCustomData_Test()
        {
            // Arrange
            var prop = new CsdlProperty();

            // Act
            // Assert
            Assert.IsNotNull(prop.CustomData);
        }

        [TestMethod]
        public void CsdlProperty_SetCustomData_Test()
        {
            // Arrange
            var prop = new CsdlProperty();
            var expectedCustomData = new SortedConcurrentDictionary<string, object>();
            prop.CustomData = expectedCustomData;

            // Act
            // Assert
            Assert.AreEqual(expectedCustomData, prop.CustomData);
        }
    }
}
