using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;
using Rhyous.Odata.Tests;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests
{
    [TestClass]
    public class FilterTests
    {
        #region Filter Tests
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
        public void Filter_HasSubFilters_False_Test()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id", Method = "eq", Right = "1" };

            // Act & Assert
            Assert.IsFalse(filter.HasSubFilters);
        }

        [TestMethod]
        public void Filter_HasSubFilters_True_Test()
        {
            // Arrange
            Filter<Entity1> filterLeft = new Filter<Entity1> { Left = "Id", Method = "eq", Right = "1" };
            Filter<Entity1> filterRight = new Filter<Entity1> { Left = "Id", Method = "eq", Right = "2" };
            Filter<Entity1> filter = new Filter<Entity1> { Left = filterLeft, Method = "OR", Right = filterRight };

            // Act & Assert
            Assert.IsTrue(filter.HasSubFilters);
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
        #endregion

        #region ToString
        [TestMethod]
        public void Filter_ToString_NonFilter_Test()
        {
            // Arrange
            var str = "Test";
            Filter<Entity1> filter = str;

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.AreEqual(str, actual);
        }


        [TestMethod]
        public void Filter_ToString_Id_eq_27_Test()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id", Method = "eq", Right = "27"};

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.AreEqual("Id eq 27", actual);
        }

        [TestMethod]
        public void Filter_ToString_Array_Test()
        {
            // Arrange
            Filter<Entity1> filter = new[] { 1, 2, 3 };

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.AreEqual("(1,2,3)", actual);
        }
        #endregion

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
            Assert.IsFalse(f.IsSimpleString);
            Assert.IsTrue(f.Left.IsSimpleString);
            Assert.AreEqual("eq", f.Method);
            Assert.IsTrue(f.Right.IsSimpleString);
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilter_Wrapped_ValidTest()
        {
            // Arrange
            string s = "'Id eq 1'"; // What if there was a database of filters and we wanted to find a filter in it?

            // Act
            Filter<User> f = s;

            // Assert
            Assert.AreEqual(s, f.ToString());
            Assert.IsTrue(f.IsSimpleString);
            Assert.IsNull(f.Left);
            Assert.IsNull(f.Method);
            Assert.IsNull(f.Right);
        }

        [TestMethod]
        public void ImplicitOperatorStringToFilter_EachPartQuoted_SoItLooksWrappedButIsNot_ValidTest()
        {
            // Arrange
            string s = "'Id' eq '1'"; // What if there was a database of filters and we wanted to find a filter in it?
            var expected = "Id eq 1";
            // Act
            Filter<User> f = s;

            // Assert
            Assert.AreEqual(expected, f.ToString());
            Assert.IsFalse(f.IsSimpleString);
            Assert.IsTrue(f.Left.IsSimpleString);
            Assert.AreEqual("eq", f.Method);
            Assert.IsTrue(f.Right.IsSimpleString);
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

        #region Parent tests
        [TestMethod]
        public void ParentCannotBeSameAsChildTest()
        {
            // Arrange
            Filter<User> f1 = new Filter<User>();

            // Act & Assert
            Assert.ThrowsException<Exception>(() => { f1.Parent = f1; });
        }
        #endregion

        #region IEnumerable tests

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string, int>>), @"Data\IEnumerableQueryStrings.json")]
        public async Task Filter_IEnumerable_Count(TestDataRow<string, int> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = new FilterExpressionParser<Entity1>(FilterExpressionParserActionDictionary<Entity1>.Instance);
            var filter = await parser.ParseAsFilterAsync(filterstring, true);

            // Act
            var count = filter.Count();

            // Assert
            Assert.AreEqual(expected, count, message);
        }
        #endregion
    }
}
