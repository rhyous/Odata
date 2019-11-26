using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.UnitTesting;

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
        [JsonTestDataSource(typeof(string), @"Data\JsonNullEmptyWhitespace.json")]
        public void GetValue_JRaw_Null_Test(Row<string> row)
        {
            // Arrange
            var json = row.Value;
            JRaw raw = (json == null) ? null : new JRaw(json);

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => raw.GetValue("Id"));
        }

        [TestMethod]
        [JsonTestDataSource(typeof(string), @"Data\JsonNullEmptyWhitespace.json")]
        public void GetValue_Property_NullEmptyOrWhitespace_Test(Row<string> row)
        {
            // Arrange
            var prop = row.Value.ToString();
            var msg = row.Message ?? row.Description;
            var raw = new JRaw("{ \"Id\" : 1, \"Prop1\" : \"Abc123\" }");

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => raw.GetValue(prop), msg);
        }
    }
}