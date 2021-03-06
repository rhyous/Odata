﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Extensions
{

    [TestClass]
    public class NameValueCollectionExtensionsTests
    {
        [TestMethod]
        public void GetFilterExpressionNullTest()
        {
            // Arrange
            NameValueCollection collection = null;

            // Act
            var actual = collection.GetFilterExpression<TestClass>();

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetFilterExpressionEmptyTest()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();

            // Act
            var actual = collection.GetFilterExpression<TestClass>();

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetFilterExpressionNotEmptyButNoFilterTest()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            collection.Add("Key1", "Value1");

            // Act
            var actual = collection.GetFilterExpression<TestClass>();

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetFilterExpression_Id_Equals_Test()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            collection.Add("$filter", "Id eq 1");
            var t1 = new TestClass { Id = 1 };
            var t2 = new TestClass { Id = 2 };
            var list = new List<TestClass> { t1, t2 };
            
            // Act
            var expression = collection.GetFilterExpression<TestClass>();
            var result = list.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.AreEqual("e => (e.Id == 1)", expression.ToString());
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(t1, result[0]);
        }

        [TestMethod]
        public void GetFilterExpression_Id_GreaterThanOrEquals_Test()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            collection.Add("$filter", "Id ge 1");
            var t1 = new TestClass { Id = 1 };
            var t2 = new TestClass { Id = 2 };
            var t3 = new TestClass { Id = 3 };
            var list = new List<TestClass> { t1, t2, t3 };

            // Act
            var expression = collection.GetFilterExpression<TestClass>();
            var result = list.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.AreEqual("e => (e.Id >= 1)", expression.ToString());
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(t1, result[0]);
            Assert.AreEqual(t2, result[1]);
            Assert.AreEqual(t3, result[2]);
        }

        [TestMethod]
        public void GetFilterExpression_Id_GreaterThan_Test()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            collection.Add("$filter", "Id gt 1");
            var t1 = new TestClass { Id = 1 };
            var t2 = new TestClass { Id = 2 };
            var t3 = new TestClass { Id = 3 };
            var list = new List<TestClass> { t1, t2, t3 };

            // Act
            var expression = collection.GetFilterExpression<TestClass>();
            var result = list.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.AreEqual("e => (e.Id > 1)", expression.ToString());
            Assert.AreEqual(2, result.Count);            
            Assert.AreEqual(t2, result[0]);
            Assert.AreEqual(t3, result[1]);
        }

        [TestMethod]
        public void GetFilterExpression_CreateDate_GreaterThan_Test()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            var dateTimeOffsetzzz = DateTime.Now.ToString("zzz");
            collection.Add("$filter", $"CreateDate gt '1-1-2020 00:00:00 {dateTimeOffsetzzz}'");
            var t1 = new TestClass { Id = 1, CreateDate = DateTimeOffset.Parse($"12-10-2019 00:00:00 {dateTimeOffsetzzz}") };
            var t2 = new TestClass { Id = 2, CreateDate = DateTimeOffset.Parse($"1-2-2020 00:00:00 {dateTimeOffsetzzz}") };
            var t3 = new TestClass { Id = 3, CreateDate = DateTimeOffset.Parse($"1-4-2020 00:00:00 {dateTimeOffsetzzz}") };
            var list = new List<TestClass> { t1, t2, t3 };
            var expected = $"e => (e.CreateDate > 1/1/2020 12:00:00 AM {dateTimeOffsetzzz})";

            // Act
            var expression = collection.GetFilterExpression<TestClass>();
            var result = list.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.AreEqual(expected, expression.ToString());
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(t2, result[0]);
            Assert.AreEqual(t3, result[1]);
        }

        [TestMethod]
        public void GetFilterExpression_CreateDate_GreaterThan_Symbol_Test()
        {
            // Arrange
            NameValueCollection collection = new NameValueCollection();
            var dateTimeOffsetzzz = DateTime.Now.ToString("zzz");
            collection.Add("$filter", $"CreateDate > '1-1-2020 00:00:00 {dateTimeOffsetzzz}'");
            var t1 = new TestClass { Id = 1, CreateDate = DateTimeOffset.Parse($"12-10-2019 00:00:00 {dateTimeOffsetzzz}") };
            var t2 = new TestClass { Id = 2, CreateDate = DateTimeOffset.Parse($"1-2-2020 00:00:00 {dateTimeOffsetzzz}") };
            var t3 = new TestClass { Id = 3, CreateDate = DateTimeOffset.Parse($"1-4-2020 00:00:00 {dateTimeOffsetzzz}") };
            var list = new List<TestClass> { t1, t2, t3 };
            var expected = $"e => (e.CreateDate > 1/1/2020 12:00:00 AM {dateTimeOffsetzzz})";

            // Act
            var expression = collection.GetFilterExpression<TestClass>();
            var result = list.AsQueryable().Where(expression).ToList();

            // Assert
            Assert.AreEqual(expected, expression.ToString());
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(t2, result[0]);
            Assert.AreEqual(t3, result[1]);
        }
    }
}
