using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests
{
    [TestClass]
    public class CsdlExtensionTests
    {
        [TestMethod]
        public void CsdlExtensions_ToCsdl_TypeNull_Tests()
        {
            // Arrange
            Type type = null;

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.IsNull(csdl);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Valid_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(Person).ToCsdl();

            // Assert
            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);

            var expectedPropertyCount = typeof(Person).GetProperties().Length;
            expectedPropertyCount += typeof(Person).GetProperties().Where(p => p.GetCustomAttribute<RelatedEntityAttribute>() != null).Count();
            expectedPropertyCount += typeof(Person).GetCustomAttributes<RelatedEntityForeignAttribute>().Count();
            expectedPropertyCount += typeof(Person).GetCustomAttributes<RelatedEntityMappingAttribute>().Count();
            Assert.AreEqual(expectedPropertyCount, csdl.Properties.Count);

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
        public void CsdlExtensions_ToCsdl_Enum_And_Double_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(SuiteMembership).ToCsdl();

            // Assert
            var expectedPropertyCount = typeof(SuiteMembership).GetProperties().Length;
            expectedPropertyCount += typeof(SuiteMembership).GetProperties().Where(p => p.GetCustomAttribute<RelatedEntityAttribute>() != null).Count();
            expectedPropertyCount += typeof(SuiteMembership).GetCustomAttributes<RelatedEntityForeignAttribute>().Count();
            expectedPropertyCount += typeof(SuiteMembership).GetCustomAttributes<RelatedEntityMappingAttribute>().Count();
            Assert.AreEqual(expectedPropertyCount, csdl.Properties.Count);

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
                Assert.AreEqual(e, (csdl.Properties["QuantityType"] as CsdlEnumProperty).CustomData[e.ToString()]);
            }

        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_PropertyInfoNull_Tests()
        {
            // Arrange
            PropertyInfo propInfo = null;

            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.IsNull(csdl);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_PropertyInfoNotInDictionary_Tests()
        {
            // Arrange
            PropertyInfo propInfo = typeof(Fake).GetProperty("TestProp");

            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.IsNull(csdl);
        }

        class Fake { Fake TestProp { get; set; } }
    }
}
