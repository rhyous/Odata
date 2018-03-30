using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests.Extensions
{
    [TestClass]
    public class JRawExtensionsTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GetValueAsStringTest()
        {
            // Arrange
            var raw = new JRaw("{ \"Id\" : 1, \"Prop1\" : \"Abc123\" }");

            // Act
            var value = raw.GetValueAsString("Id");

            // Assert
            Assert.AreEqual("1", value);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\JsonNullEmptyWhitespace.xml", "Row", DataAccessMethod.Sequential)]
        public void GetValue_JRaw_Null_Test()
        {
            // Arrange
            var json = TestContext.DataRow[0].ToString().Trim('"');
            JRaw raw = (json == "null") ? null : new JRaw(json);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => raw.GetValue("Id"));
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\JsonNullEmptyWhitespace.xml", "Row", DataAccessMethod.Sequential)]

        public void GetValue_Property_NullEmptyOrWhitespace_Test()
        {
            // Arrange
            var prop = TestContext.DataRow[0].ToString().Trim('"');
            if (prop == "null")
                prop = null;
            var msg = TestContext.DataRow[1].ToString();
            var raw = new JRaw("{ \"Id\" : 1, \"Prop1\" : \"Abc123\" }");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => raw.GetValue(prop), msg);
        }
    }
}