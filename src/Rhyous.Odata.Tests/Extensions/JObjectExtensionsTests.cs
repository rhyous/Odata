using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests.Extensions
{
    [TestClass]
    public class JObjectExtensionsTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\JsonNullEmptyWhitespace.xml", "Row", DataAccessMethod.Sequential)]
        public void GetIdDynamic_IdProperty_Null_Empty_Whitespace_Test()
        {
            // Arrange
            var value = TestContext.DataRow[0].ToString();
            var msg = TestContext.DataRow[1].ToString();
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
