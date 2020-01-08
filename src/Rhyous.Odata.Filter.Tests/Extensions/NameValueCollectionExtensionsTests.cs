using System;
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
    }
}
