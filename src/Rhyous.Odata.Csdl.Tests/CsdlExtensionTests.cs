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
    }
}
