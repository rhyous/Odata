using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;

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
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\NaiveQueryStrings.json")]
        public void FilterExpressionParser_NaiveFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\ComplexQueryStrings.json")]
        public void FilterExpressionParser_ComplexFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\GroupedQueryStrings.json")]
        public void FilterExpressionParser_GroupedFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var parser = new FilterExpressionParser<Entity1>();

            // Act
            var actual = parser.Parse(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\StringMethodQueryStrings.json")]
        public void FilterExpressionParser_StringMethodFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
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