using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class FilterExpressionBuilderTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\NaiveQueryStrings.json")]
        public void NaiveFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        public void ComplexFilterParserTests_TmpTest()
        {
            // Arrange
            var filterstring = "Name eq \"Runnin''\"";
            var expected = "e => (e.Name == \"Runnin'\")";
            var message = "";
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\ComplexQueryStrings.json")]
        public void ComplexFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();            

            // Assert
            Assert.AreEqual(expected, actual, message);
           
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\GroupedQueryStrings.json")]
        public void GroupedFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();
            
            // Assert
            Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\StringMethodQueryStrings.json")]
        public void StringMethodFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual, message);
        }

        [TestMethod]
        public void UnclosedGroupThrowsException()
        {
            // Arrange
            var filterstring = "Name eq O'Brien";
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act && Assert
            Assert.ThrowsException<InvalidFilterSyntaxException>(() => builder.Expression);
        }


        [TestMethod]
        public void UnclosedParanethesisThrowsException()
        {
            // Arrange
            var filterstring = "(Id eq 1";
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act && Assert
            Assert.ThrowsException<InvalidFilterSyntaxException>(() => builder.Expression);
        }


        [TestMethod]
        public void CloseParanethesisWithoutOpenParanethesisThrowsException()
        {
            // Arrange
            var filterstring = "Id eq 1)";
            var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            // Act && Assert
            Assert.ThrowsException<InvalidGroupingException>(() => builder.Expression);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\DateTimeQueryStrings.json")]
        public void DateTimeFilterParserTests(Row<string> row)
        {
            // Arrange
            var filterstring = row.Value;
            var expected = row.Expected;
            var message = row.Message;
            var builder = new FilterExpressionBuilder<TestClass>(filterstring, new FilterExpressionParser<TestClass>());

            // Act
            var actual = builder.Expression.ToString();

            // Assert
            Assert.AreEqual(expected, actual, message);
        }
    }
}
