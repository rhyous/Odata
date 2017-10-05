using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class FilterExpressionBuilderTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\NaiveQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void NaiveFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\ComplexQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void ComplexFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();
            

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\StringMethodQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void StringMethodFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();


            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
