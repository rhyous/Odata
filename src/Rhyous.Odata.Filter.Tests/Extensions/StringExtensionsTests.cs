using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using Rhyous.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.Odata.Filter.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\Constants.json")]
        public void StringExtensions_EnforceConstant_IsConstant_ReturnsSameValue_Test(Row<string> row)
        {
            // Arrange
            var constant = row.Value;
            var message = string.Format(row.Message, constant);

            // Act
            var actual = constant.EnforceConstant<TestClass>();

            // Assert
            Assert.AreEqual(constant, actual);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\NaiveQueryStrings.json")]
        public void StringExtensions_EnforceConstant_IsNotConstant_Throws_Test(Row<string> row)
        {
            // Arrange
            var strExpression = row.Value;
            var message = row.Message;

            // Act
            // Assert
            Assert.ThrowsException<InvalidOdataConstantException>(() =>
            {
                strExpression.EnforceConstant<Entity1>();
            }, message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<Row<string>>), @"Data\OdataQueryInjectionAttempts.json")]
        public void StringExtensions_EnforceConstant_AppendingQueryToConstant_Throws_Test(Row<string> row)
        {
            // Arrange
            var strExpression = row.Value;
            var message = row.Message;

            // Act
            // Assert
            Assert.ThrowsException<InvalidOdataConstantException>(() =>
            {
                strExpression.EnforceConstant<Entity1>();
            });
        }
    }
}
