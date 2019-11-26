using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.UnitTesting;

namespace Rhyous.Odata.Tests.Extensions
{
    [TestClass]
    public class JObjectExtensionsTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [JsonTestDataSource(typeof(string), @"Data\JsonNullEmptyWhitespace.json")]
        public void GetIdDynamic_IdProperty_Null_Empty_Whitespace_Test(Row<string> row)
        {
            // Arrange
            var value = row.Value;
            var msg = row.Message ?? row.Description;
            var json = $"{{ \"Id\" : 1, \"AltId\" : \"ABC1\", \"IdProperty\" : {value} }}";
            var obj = JObject.Parse(json);

            // Act
            var actual = obj.GetIdDynamic(Constants.IdPropertySpeficier);

            // Assert
            Assert.AreEqual("1", actual, msg);
        }

        [TestMethod]
        public void GetIdDynamic_IdProperty_Id_Test()
        {
            // Arrange
            var obj = JObject.Parse("{ \"Id\" : 1, \"AltId\" : \"ABC1\", \"IdProperty\" : \"Id\" }");

            // Act
            var actual = obj.GetIdDynamic(Constants.IdPropertySpeficier);

            // Assert
            Assert.AreEqual("1", actual);
        }

        [TestMethod]
        public void GetIdDynamic_AltId_Valid_Test()
        {
            // Arrange
            var obj = JObject.Parse("{ \"Id\" : 1, \"AltId\" : \"ABC1\", \"IdProperty\" : \"AltId\" }");

            // Act
            var actual = obj.GetIdDynamic(Constants.IdPropertySpeficier);

            // Assert
            Assert.AreEqual("ABC1", actual);
        }
    }
}
