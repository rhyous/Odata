using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Parsers
{
    [TestClass]
    public class FilterExpressionParserTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void FilterExpressionParser_SimpleParserTest()
        {
            // Arrange
            var parser = new FilterExpressionParser<IUser>();
            var expression = "Id eq 1";
            string expected = "e => (e.Id == 1)";

            // Act
            var actual = parser.Parse(expression);

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\NaiveQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void FilterExpressionParser_NaiveFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\ComplexQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void FilterExpressionParser_ComplexFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\GroupedQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void FilterExpressionParser_GroupedFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\StringMethodQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void FilterExpressionParser_StringMethodFilterParserTests()
        {
            // Arrange
            var filterstring = TestContext.DataRow["Query"].ToString();
            var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            var message = TestContext.DataRow["Message"].ToString();
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }


        [TestMethod]
        public void FilterExpressionParser_TwoAnds_FilterParserTests()
        {
            // Arrange
            var filterstring = "Entity eq Organization and EntityId eq 265932 and Property eq ABC";
            var expected = "e => ((e.Entity == \"Organization\") AndAlso ((e.EntityId == \"265932\") AndAlso (e.Property == \"ABC\")))";
            var message = "";
            var parser = new FilterExpressionParser<AlternateId>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        public void FilterExpressionParser_Enum_eq_FilterParserTests()
        {
            // Arrange
            var filterstring = "Type eq 1";
            var expected = "e => (e.Type == Cool)";
            var message = "";
            var parser = new FilterExpressionParser<EntityWithEnum>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        public void FilterExpressionParser_Enum_Contains_FilterParserTests()
        {
            // Arrange
            var filterstring = "Contains(Type, 1)";
            var parser = new FilterExpressionParser<EntityWithEnum>();

            // Act & Assert
            Assert.ThrowsException<InvalidTypeMethodException>(() => { parser.Parse(filterstring); }); ;
        }
    }
}