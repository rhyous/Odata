using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Csdl.Tests
{
    [TestClass]
    public class CsdlExtensionTests
    {
        [TestMethod]
        public void ToCsdlTests()
        {
            // Arrange
            // Act
            var csdl = typeof(Person).ToCsdl<Person>();

            // Assert
            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);
            Assert.AreEqual(typeof(Person).GetProperties().Length, csdl.Properties.Count);

            Assert.AreEqual("Id", csdl.Properties[0].Name);
            Assert.AreEqual(1, csdl.Properties[0].CsdlType.Count);
            Assert.AreEqual("int32", csdl.Properties[0].CsdlFormat);

            Assert.AreEqual("FirstName", csdl.Properties[1].Name);
            Assert.AreEqual(1, csdl.Properties[1].CsdlType.Count);
            Assert.AreEqual("", csdl.Properties[1].CsdlFormat);

            Assert.AreEqual("LastName", csdl.Properties[2].Name);
            Assert.AreEqual(1, csdl.Properties[2].CsdlType.Count);
            Assert.AreEqual("", csdl.Properties[2].CsdlFormat);

            Assert.AreEqual("DateOfBirth", csdl.Properties[3].Name);
            Assert.AreEqual(1, csdl.Properties[3].CsdlType.Count);
            Assert.AreEqual("date", csdl.Properties[3].CsdlFormat);
        }

        [TestMethod]
        public void ToCsdl_Enum_And_Double_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(SuiteMembership).ToCsdl<SuiteMembership>();

            // Assert
            Assert.AreEqual(typeof(SuiteMembership).GetProperties().Length, csdl.Properties.Count);

            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);
            Assert.AreEqual("Id", csdl.Properties[0].Name);
            Assert.AreEqual(1, csdl.Properties[0].CsdlType.Count);
            Assert.AreEqual("int32", csdl.Properties[0].CsdlFormat);

            Assert.AreEqual("SuiteId", csdl.Properties[1].Name);
            Assert.AreEqual(1, csdl.Properties[1].CsdlType.Count);
            Assert.AreEqual("int32", csdl.Properties[1].CsdlFormat);

            Assert.AreEqual("ProductId", csdl.Properties[2].Name);
            Assert.AreEqual(1, csdl.Properties[2].CsdlType.Count);
            Assert.AreEqual("int32", csdl.Properties[2].CsdlFormat);
            
            Assert.AreEqual("Quantity", csdl.Properties[3].Name);
            Assert.AreEqual(1, csdl.Properties[3].CsdlType.Count);
            Assert.AreEqual("double", csdl.Properties[3].CsdlFormat);
            
            Assert.AreEqual("QuantityType", csdl.Properties[4].Name);
            Assert.AreEqual(1, csdl.Properties[4].CsdlType.Count);
            Assert.AreEqual("int32", csdl.Properties[4].CsdlFormat);
        }
    }
}
