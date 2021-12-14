using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter.Tests.Parsers
{
    [TestClass]
    public class FilterToExpressionConverterTests
    {
        private FilterToExpressionConverter CreateFilterToExpressionConverter()
        {
            return FilterToExpressionConverter.Instance as FilterToExpressionConverter;
        }

        #region Convert
        [TestMethod]
        public void FilterToExpressionConverter_Convert_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var filter = new Filter<User> { Left = "Id", Method = "eq", Right = "1" };

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => (e.Id == 1)", expressionString);
        }

        [TestMethod]
        public void FilterToExpressionConverter_Convert_Operator_IN_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var filter = new Filter<User> { Left = "Id", Method = "in", Right = new[] {1,2,3 } };

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var users = new List<User>
            {
                new User{ Id = 1},
                new User{ Id = 2},
                new User{ Id = 7},
            };
            var usersFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => value(System.Int32[]).Contains(e.Id)", expressionString);
            Assert.AreEqual(2, usersFound.Count);
            Assert.AreEqual(1, usersFound[0].Id);
            Assert.AreEqual(2, usersFound[1].Id);
        }

        [TestMethod]
        public void FilterToExpressionConverter_Convert_Operator_NOT_IN_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var filter = new Filter<User> { Left = "Id", Method = "in", Right = new[] { 1, 2, 3 }, Not = true };

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var users = new List<User>
            {
                new User{ Id = 1},
                new User{ Id = 2},
                new User{ Id = 7},
            };
            var usersNotFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => Not(value(System.Int32[]).Contains(e.Id))", expressionString);
            Assert.AreEqual(1, usersNotFound.Count);
            Assert.AreEqual(7, usersNotFound[0].Id);
        }
        #endregion

        
    }
}
