using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter.Tests.Builder
{
    [TestClass]
    public class ConjunctionExpressionBuilderTests
    {
        private MockRepository _MockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


        }

        private ConjunctionExpressionBuilder CreateConjunctionExpressionBuilder()
        {
            return new ConjunctionExpressionBuilder();
        }

        #region Build
        [TestMethod]
        public void ConjunctionExpressionBuilder_Build_Conjuction_Test()
        {
            // Arrange
            var conjunctionExpressionBuilder = CreateConjunctionExpressionBuilder();
            Conjunction conj = Conjunction.Or;
            Filter<User> firstFilter = new Filter<User> { Left = "Id", Method = "eq", Right = "1" };
            Filter<User> secondFilter = new Filter<User> { Left = "Id", Method = "eq", Right = "2" };
            var filter = new Filter<User> { Left = firstFilter, Method = conj.ToString(), Right = secondFilter };

            var users = new List<User>
            {
                new User{ Id = 1},
                new User{ Id = 2},
                new User{ Id = 7}
            };

            // Act
            var result = conjunctionExpressionBuilder.Build(filter, conj);
            var usersFound = users.Where(result.Compile())?.ToList();
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => ((e.Id == 1) OrElse (e.Id == 2))", expressionString);
            Assert.AreEqual(1, usersFound[0].Id);
            Assert.AreEqual(2, usersFound[1].Id);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetCombinedExpression tests
        [TestMethod]
        public void FilterGetCombinedExpressionBothNull()
        {
            // Arrange
            var conjunctionExpressionBuilder = CreateConjunctionExpressionBuilder();
            Filter<User> f1 = null;
            Filter<User> f2 = null;

            // Act
            var s = conjunctionExpressionBuilder.GetCombinedExpression<User>(f1, f2, Conjunction.And);

            // Assert
            Assert.IsNull(s);
        }

        [TestMethod]
        public void FilterGetCombinedExpressionRightNull()
        {
            // Arrange
            var conjunctionExpressionBuilder = CreateConjunctionExpressionBuilder();
            var f1 = new Filter<User>
            {
                Left = "Id",
                Method = "eq",
                Right = "1"
            };
            var f1Expression = (Expression<Func<User, bool>>)f1;
            Expression<Func<User, bool>> f2Expression = null;

            // Act
            var s = conjunctionExpressionBuilder.GetCombinedExpression<User>(f1Expression, f2Expression, Conjunction.And);

            // Assert
            Assert.AreEqual(f1Expression, s);
        }

        [TestMethod]
        public void FilterGetCombinedExpressionLeftNull()
        {
            // Arrange
            var conjunctionExpressionBuilder = CreateConjunctionExpressionBuilder();
            var f2 = new Filter<User>
            {
                Left = "Id",
                Method = "eq",
                Right = "1"
            };
            var f2Expression = (Expression<Func<User, bool>>)f2;
            Expression<Func<User, bool>> f1Expression = null;

            // Act
            var s = conjunctionExpressionBuilder.GetCombinedExpression<User>(f1Expression, f2Expression, Conjunction.And);

            // Assert
            Assert.AreEqual(f2Expression, s);
        }
        #endregion
    }
}
