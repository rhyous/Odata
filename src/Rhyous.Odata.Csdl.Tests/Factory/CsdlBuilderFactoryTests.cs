using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Csdl.Tests.Factory
{
    [TestClass]
    public class CsdlBuilderFactoryTests
    {
        [TestMethod]
        public void GetPropertyBuilder_CreateEntityBuilder_Test()
        {
            // Arrange
            var unitUnderTest = new CsdlBuilderFactory();

            // Act
            var result = unitUnderTest.CreateEntityBuilder();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPropertyBuilder_CreatePropertyBuilder_Test()
        {
            // Arrange
            var unitUnderTest = new CsdlBuilderFactory();

            // Act
            var result = unitUnderTest.CreatePropertyBuilder();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEnumPropertyBuilder_CreateEnumPropertyBuilder_Test()
        {
            // Arrange
            var unitUnderTest = new CsdlBuilderFactory();

            // Act
            var result = unitUnderTest.CreateEnumPropertyBuilder();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
