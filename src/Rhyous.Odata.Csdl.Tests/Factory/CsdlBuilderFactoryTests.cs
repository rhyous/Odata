using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;

namespace Rhyous.Odata.Csdl.Tests.Factory
{
    [TestClass]
    public class CsdlBuilderFactoryTests
    {
        private MockRepository _MockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


        }

        private CsdlBuilderFactory CreateFactory()
        {
            return new CsdlBuilderFactory();
        }

        #region Tests
        [TestMethod]
        public void GetPropertyBuilder_CreateEntityBuilder_Test()
        {
            // Arrange
            var unitUnderTest = CreateFactory();

            // Act
            var result = unitUnderTest.EntityBuilder;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetPropertyBuilder_CreatePropertyBuilder_Test()
        {
            // Arrange
            var unitUnderTest = CreateFactory();

            // Act
            var result = unitUnderTest.PropertyBuilder;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetEnumPropertyBuilder_CreateEnumPropertyBuilder_Test()
        {
            // Arrange
            var unitUnderTest = CreateFactory();

            // Act
            var result = unitUnderTest.EnumPropertyBuilder;

            // Assert
            Assert.IsNotNull(result);
        }
        #endregion
    }
}
