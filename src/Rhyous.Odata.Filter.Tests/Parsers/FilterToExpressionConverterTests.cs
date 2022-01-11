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
            var filter = new Filter<User> { Left = "Id", Method = "in", Right = new[] { 1, 2, 3 } };

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
                new User{ Id = 1 },
                new User{ Id = 2 },
                new User{ Id = 7 },
            };
            var usersNotFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => Not(value(System.Int32[]).Contains(e.Id))", expressionString);
            Assert.AreEqual(1, usersNotFound.Count);
            Assert.AreEqual(7, usersNotFound[0].Id);
        }

        [TestMethod]
        public void FilterToExpressionConverter_Convert_ArrayItemTypeString_Operator_IN_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var array = new[] { "John Doe" , "Jane Doe" };
            var filter = new Filter<User> { Left = "Name", Method = "in", Right = array };

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe"},
                new User { Id = 2, Name = "Jane Doe" },
                new User { Id = 7, Name = "Bob Bobinski"},
            };
            var usersFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => value(System.String[]).Contains(e.Name)", expressionString);
            Assert.AreEqual(2, usersFound.Count);
            Assert.AreEqual(1, usersFound[0].Id);
            Assert.AreEqual(2, usersFound[1].Id);
        }

        [TestMethod]
        public void FilterToExpressionConverter_Convert_ArrayItemTypeString_Parsed_Operator_IN_Quoted_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var array = new[] { "John Doe", "Jane Doe" };
            Filter<User> filter = "Name in ('John Doe','Jane Doe')";

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe"},
                new User { Id = 2, Name = "Jane Doe" },
                new User { Id = 7, Name = "Bob Bobinski"},
            };
            var usersFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => value(System.String[]).Contains(e.Name)", expressionString);
            Assert.AreEqual(2, usersFound.Count);
            Assert.AreEqual(1, usersFound[0].Id);
            Assert.AreEqual(2, usersFound[1].Id);
        }

        /// <summary>
        /// This tests shows how it currently works. But it begs the questions:
        /// Should a quoted string in a string array be unquoted?
        /// Should we look for both the string with or without the quotes?
        /// Or just let the user realize this and fix it? (currently the choice)
        /// </summary>
        /// <remarks>In the unit test above, the parser strips out the quotes. However, you could 
        /// simulate this by parsing this string: "Name in ('John Doe',\"'Jane Doe'\")"</remarks>
        [TestMethod]
        public void FilterToExpressionConverter_Convert_ArrayItemTypeString_ExactArray_Operator_IN_Test()
        {
            // Arrange
            var filterToExpressionConverter = CreateFilterToExpressionConverter();
            var array = new[] { "John Doe", "'Jane Doe'" };
            var filter = new Filter<User> { Left = "Name", Method = "in", Right = array };

            // Act
            var result = filterToExpressionConverter.Convert(filter);
            var users = new List<User>
            {
                new User { Id = 1, Name = "John Doe"},
                new User { Id = 2, Name = "Jane Doe" },
                new User { Id = 7, Name = "Bob Bobinski"},
            };
            var usersFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => value(System.String[]).Contains(e.Name)", expressionString);
            Assert.AreEqual(1, usersFound.Count);
            Assert.AreEqual(1, usersFound[0].Id);
        }
        #endregion


    }
}
