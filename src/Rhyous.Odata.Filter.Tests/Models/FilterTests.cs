﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void FilterLengthTest()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id", Right = "1", Method = "10"};
            var expected = 7;

            // Act
            var actual = filter.Length;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FilterNonFilterLengthTest()
        {
            // Arrange
            Filter<Entity1> filter = "test";
            var expected = 4;

            // Act
            var actual = filter.Length;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FilterIsCompleteFalseTest()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id" };

            // Act & Assert
           Assert.IsFalse(filter.IsComplete);
        }

        [TestMethod]
        public void FilterIsCompleteTrueTest()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id", Right = "1", Method = "10" };

            // Act & Assert
            Assert.IsTrue(filter.IsComplete);
        }

        [TestMethod]
        public void FilterIsCompleteNonFilterTrueTest()
        {
            // Arrange
            Filter<Entity1> filter = "test";

            // Act & Assert
            Assert.IsTrue(filter.IsComplete);
        }

        [TestMethod]
        public void SafeSetValueNullToExistingNull()
        {
            // Arrange
            Filter<Entity1> filter = "test";

            // Act
            string s = null;
            filter.SafeSet(null, ref s);

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void SafeSetValueNullToExistingValue()
        {
            // Arrange
            Filter<Entity1> filter = "test";

            // Act
            string s = "a";
            filter.SafeSet(null, ref s);

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void SafeSetValueParent()
        {
            // Arrange
            Filter<Entity1> filter = "test";

            // Act
            A backingField = null;
            A child = new A();
            A parent = new A();
            filter.SafeSet(child, ref backingField, parent);

            // Assert
            Assert.AreEqual(backingField, child);
            Assert.AreEqual(child.Parent, parent);
        }
        
        public class A : IParent<A>
        {
            public A Parent { get ; set; }
        }

        #region Imlicit operator cast Filter to string tests
        [TestMethod]
        public void ImplicitOperatorFilterToStringNullTest()
        {
            // Arrange
            Filter<User> f = null;

            // Act
            string s = f;

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void ImplicitOperatorFilterToStringDefaultTest()
        {
            // Arrange
            Filter<User> f = new Filter<User>();

            // Act
            string s = f;

            // Assert
            Assert.IsNull(s);
        }
        #endregion

        #region Imlicit operator cast string to filter tests
        [TestMethod]
        public void ImplicitOperatorStringToFilterNullTest()
        {
            // Arrange
            string s = null;

            // Act
            Filter<User> f = s;

            // Assert
            Assert.IsNull(f);
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilterEmptyTest()
        {
            // Arrange
            string s = "";

            // Act
            Filter<User> f = s;

            // Assert
            Assert.IsNull(f);
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilterWhitespaceTest()
        {
            // Arrange
            string s = "   ";

            // Act
            Filter<User> f = s;

            // Assert
            Assert.IsNull(f);
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilterGarbageTest()
        {
            // Arrange
            string s = "asdfsaldkjflsjd;lj;asd032498r304a;sldkcn";

            // Act
            Filter<User> f = s;

            // Assert
            Assert.AreEqual(s, f.ToString());
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilterValidTest()
        {
            // Arrange
            string s = "Id eq 1";

            // Act
            Filter<User> f = s;

            // Assert
            Assert.AreEqual(s, f.ToString());
            Assert.IsTrue(f.IsSimpleString);
        }
        #endregion

        #region Imlicit operator cast Filter to Expression<Func<T, bool>> tests
        [TestMethod]
        public void ImplicitOperatorFilterToExpressionFuncTBoolNullTest()
        {
            // Arrange
            Filter<User> f = null;

            // Act
            Expression<Func<User,bool>> s = f;

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void ImplicitOperatorFilterToExpressionFuncTBoolDefaultTest()
        {
            // Arrange
            Filter<User> f = new Filter<User>();

            // Act
            Expression<Func<User, bool>> s = f;

            // Assert
            Assert.IsNull(s);
        }
        #endregion

        #region GetCombinedExpression tests
        [TestMethod]
        public void FilterGetCombinedExpressionBothNull()
        {
            // Arrange
            Filter<User> f1 = null;
            Filter<User> f2 = null;

            // Act
            var s = Filter<User>.GetCombinedExpression(f1, f2, Conjunction.And);

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void FilterGetCombinedExpressionRightNull()
        {
            // Arrange
            var f1 = new Filter<User>
            {
                Left = "Id",
                Method = "eq",
                Right = "1"
            };
            var f1Expression = (Expression<Func<User, bool>>)f1;
            Expression<Func<User, bool>> f2Expression = null;

            // Act
            var s = Filter<User>.GetCombinedExpression(f1Expression, f2Expression, Conjunction.And);

            // Assert
            Assert.AreEqual(f1Expression, s);
        }

        [TestMethod]
        public void FilterGetCombinedExpressionLeftNull()
        {
            // Arrange
            var f2 = new Filter<User>
            {
                Left = "Id",
                Method = "eq",
                Right = "1"
            };
            var f2Expression = (Expression<Func<User, bool>>)f2;
            Expression<Func<User, bool>> f1Expression = null;

            // Act
            var s = Filter<User>.GetCombinedExpression(f1Expression, f2Expression, Conjunction.And);

            // Assert
            Assert.AreEqual(f2Expression, s);
        }
        #endregion

        #region Group tests


        [TestMethod]
        public void UngroupedFilterTest()
        {
            // Arrange
            var expected = "e => ((e.Id == 1) AndAlso (e.Name == \"Jared\"))";
            Filter<User> f = new Filter<User>
            {
                Left = new Filter<User>
                {
                    Left = "Id",
                    Method = "eq",
                    Right = "1"
                },
                Method = "and",
                Right = new Filter<User>
                {
                    Left = "Name",
                    Method = "eq",
                    Right = "Jared"
                },
            };

            // Act
            Expression<Func<User, bool>> s = f;
            var actual = s.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GroupedFilterTest()
        {
            // Arrange
            var expected = "e => (((e.Id == 1) AndAlso (e.Name == \"Jared\")) OrElse ((e.Id == 2) AndAlso (e.Name == \"Elih\")))";
            Filter<User> f1 = new Filter<User>
            {
                Left = new Filter<User>
                {
                    Left = "Id",
                    Method = "eq",
                    Right = "1"
                },
                Method = "and",
                Right = new Filter<User>
                {
                    Left = "Name",
                    Method = "eq",
                    Right = "Jared"
                },
            };
            Filter<User> f2 = new Filter<User>
            {
                Left = new Filter<User>
                {
                    Left = "Id",
                    Method = "eq",
                    Right = "2"
                },
                Method = "and",
                Right = new Filter<User>
                {
                    Left = "Name",
                    Method = "eq",
                    Right = "Elih"
                },
            };
            var groupedFilter = new Filter<User> { Left = f1, Method = "or", Right = f2 };

            // Act
            Expression<Func<User, bool>> s = groupedFilter;
            var actual = s.ToString();

            // Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        [TestMethod]
        public void ParentCannotBeSameAsChildTest()
        {
            // Arrange
            Filter<User> f1 = new Filter<User>();

            // Act & Assert
            Assert.ThrowsException<Exception>(() => { f1.Parent = f1; });
        }
    }
}
