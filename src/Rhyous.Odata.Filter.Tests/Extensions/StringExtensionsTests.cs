using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using Rhyous.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.Odata.Filter.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        #region EnforceConstant
        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\Constants.json")]
        public void StringExtensions_EnforceConstant_IsConstant_ReturnsSameValue_Test(TestDataRow<string> row)
        {
            // Arrange
            var constant = row.TestValue;
            var message = string.Format(row.Message, constant);

            // Act
            var actual = constant.EnforceConstant<TestClass>();

            // Assert
            Assert.AreEqual(constant, actual);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\NaiveQueryStrings.json")]
        public void StringExtensions_EnforceConstant_IsNotConstant_Throws_Test(TestDataRow<string> row)
        {
            // Arrange
            var strExpression = row.TestValue;
            var message = row.Message;

            // Act
            // Assert
            Assert.ThrowsException<InvalidOdataConstantException>(() =>
            {
                strExpression.EnforceConstant<Entity1>();
            }, message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\OdataQueryInjectionAttempts.json")]
        public void StringExtensions_EnforceConstant_AppendingQueryToConstant_Throws_Test(TestDataRow<string> row)
        {
            // Arrange
            var strExpression = row.TestValue;
            var message = row.Message;

            // Act
            // Assert
            Assert.ThrowsException<InvalidOdataConstantException>(() =>
            {
                strExpression.EnforceConstant<Entity1>();
            });
        }
        #endregion

        #region IsQuotedSmart
        [TestMethod]
        public void StingExtensions_IsQuoted_Double_True_Test()
        {
            // Arrange
            var str = "\"A quoted string\"";

            // Act
            var actual = str.IsQuoted();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void StingExtensions_IsQuoted_Single_True_Test()
        {
            // Arrange
            var str = "'A quoted string'";

            // Act
            var actual = str.IsQuoted();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void StingExtensions_IsQuoted_SingleAndDouble_False_Test()
        {
            // Arrange
            var str = "'A quoted string\"";

            // Act
            var actual = str.IsQuoted();

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void StingExtensions_IsQuoted_ExperssionQuotedInPartsThatLooksFullyQuotedButIsNot_False_Test()
        {
            // Arrange
            var str = "'Id' eq '27'";

            // Act
            var actual = str.IsQuoted();

            // Assert
            Assert.IsFalse(actual);
        }
        #endregion

        #region EscapeAndQuote
        [TestMethod]
        [PrimitiveList("NoSpace","Has Space")] // Doesn't check for space, that is a seperate responsibility
        public void StingExtensions_EscapeAndQuote_Quoted_Test(string str)
        {
            // Arrange
            // Act
            var actual = str.EscapeAndQuote();

            // Assert
            Assert.AreEqual($"'{str}'", actual);
        }

        [TestMethod]
        public void StingExtensions_EscapeAndQuote_HasSingleQuote_NotQuoted_Test()
        {
            // Arrange
            string str = "O'Brien";

            // Act
            var actual = str.EscapeAndQuote();

            // Assert
            Assert.AreEqual("\"O'Brien\"", actual);
        }

        [TestMethod]
        public void StingExtensions_EscapeAndQuote_HasDoubleQuote_NotQuoted_Test()
        {
            // Arrange
            string str = "A \" B";

            // Act
            var actual = str.EscapeAndQuote();

            // Assert
            Assert.AreEqual("'A \" B'", actual);
        }

        [TestMethod]
        public void StingExtensions_EscapeAndQuote_HasSingleAndDoubleQuote_NotQuoted_Test()
        {
            // Arrange
            string str = "A '\"' B";

            // Act
            var actual = str.EscapeAndQuote();

            // Assert
            Assert.AreEqual("'A ''\"'' B'", actual);
        }
        #endregion

        #region HasWhitespace
        [TestMethod]
        public void StingExtensions_HasWhitespace_True_Test()
        {
            // Arrange
            string str = "Has Space";
            // Act
            var actual = str.HasWhitespace();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void StingExtensions_HasWhitespace_False_Test()
        {
            // Arrange
            string str = "NoSpace";
            // Act
            var actual = str.HasWhitespace();

            // Assert
            Assert.IsFalse(actual);
        }
        #endregion
    }
}
