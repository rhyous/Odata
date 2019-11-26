﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            //// Arrange
            //var filterstring = TestContext.DataRow["Query"].ToString();
            //var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            //var message = TestContext.DataRow["Message"].ToString();
            //var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            //// Act
            //var actual = builder.Expression.ToString();

            //// Assert
            //Assert.AreEqual(expected, actual, message);
            Assert.Fail();
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
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\ComplexQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void ComplexFilterParserTests()
        {
            //// Arrange
            //var filterstring = TestContext.DataRow["Query"].ToString();
            //var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            //var message = TestContext.DataRow["Message"].ToString();
            //var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            //// Act
            //var actual = builder.Expression.ToString();            

            //// Assert
            //Assert.AreEqual(expected, actual, message);
            Assert.Fail();
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\GroupedQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void GroupedFilterParserTests()
        {
            //// Arrange
            //var filterstring = TestContext.DataRow["Query"].ToString();
            //var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            //var message = TestContext.DataRow["Message"].ToString();
            //var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            //// Act
            //var actual = builder.Expression.ToString();
            
            //// Assert
            //Assert.AreEqual(expected, actual, message);
            Assert.Fail();
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", @"Data\StringMethodQueryStrings.xml", "Row", DataAccessMethod.Sequential)]
        public void StringMethodFilterParserTests()
        {
            //// Arrange
            //var filterstring = TestContext.DataRow["Query"].ToString();
            //var expected = TestContext.DataRow["ExpectedExpression"].ToString();
            //var message = TestContext.DataRow["Message"].ToString();
            //var builder = new FilterExpressionBuilder<Entity1>(filterstring, new FilterExpressionParser<Entity1>());

            //// Act
            //var actual = builder.Expression.ToString();

            //// Assert
            //Assert.AreEqual(expected, actual, message);
            Assert.Fail();
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
    }
}
