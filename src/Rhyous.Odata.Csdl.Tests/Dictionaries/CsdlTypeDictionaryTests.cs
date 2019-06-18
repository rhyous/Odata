using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class CsdlTypeDictionaryTests
    {
        [TestMethod]
        public void Init_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = new CsdlTypeDictionary();

            // Act
            unitUnderTest.Init();

            // Assert
            Assert.AreEqual(68, unitUnderTest.Count);
        }
    }
}
