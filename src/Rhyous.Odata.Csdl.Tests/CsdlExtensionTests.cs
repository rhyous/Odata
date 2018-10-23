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
            var csdl = typeof(Person).ToCsdl();

            // Assert
            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);
            Assert.AreEqual(typeof(Person).GetProperties().Length, csdl.Properties.Count);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("Id", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["Id"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("FirstName", out object _));            
            Assert.AreEqual("Edm.String", (csdl.Properties["FirstName"] as CsdlProperty).Type);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("LastName", out object _));
            Assert.AreEqual("Edm.String", (csdl.Properties["LastName"] as CsdlProperty).Type);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("DateOfBirth", out object _));
            Assert.AreEqual("Edm.Date", (csdl.Properties["DateOfBirth"] as CsdlProperty).Type);
        }

        [TestMethod]
        public void ToCsdl_Enum_And_Double_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(SuiteMembership).ToCsdl();

            // Assert
            Assert.AreEqual(typeof(SuiteMembership).GetProperties().Length, csdl.Properties.Count);

            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("Id", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["Id"] as CsdlProperty).Type);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("SuiteId", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["SuiteId"] as CsdlProperty).Type);
            
            Assert.IsTrue(csdl.Properties.TryGetValue("ProductId", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["ProductId"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("Quantity", out object _));
            Assert.AreEqual("Edm.Double", (csdl.Properties["Quantity"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("QuantityType", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["QuantityType"] as CsdlEnumProperty).UnderlyingType);

            foreach (var e in Enum.GetValues(typeof(QuantityType)))
            {
                Assert.AreEqual(e, (csdl.Properties["QuantityType"] as CsdlEnumProperty).EnumOptions[e.ToString()]);
            }

        }
    }
}
