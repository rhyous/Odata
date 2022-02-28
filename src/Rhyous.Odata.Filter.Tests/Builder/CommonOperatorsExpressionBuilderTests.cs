using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Tests;
using System;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter.Tests.Builder
{
    [TestClass]
    public class CommonOperatorsExpressionBuilderTests
    {
        private MockRepository _MockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


        }

        private CommonOperatorsExpressionBuilder CreateCommonOperatorsExpressionBuilder()
        {
            return new CommonOperatorsExpressionBuilder();
        }

        #region Build
        [TestMethod]
        public void CommonOperatorsExpressionBuilder_Build_Filter_EQ_Works()
        {
            // Arrange
            var commonOperatorsExpressionBuilder = CreateCommonOperatorsExpressionBuilder();
            Filter<User> filter = new Filter<User> { Left = "Id", Method = "eq", Right = "1" };
            CommonOperatorExpressionMethods.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func);

            // Act
            var result = commonOperatorsExpressionBuilder.Build(filter, func);
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => (e.Id == 1)", expressionString);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CommonOperatorsExpressionBuilder_Build_Filter_NEQ_Works()
        {
            // Arrange
            var commonOperatorsExpressionBuilder = CreateCommonOperatorsExpressionBuilder();
            Filter<User> filter = new Filter<User> { Left = "Id", Method = "ne", Right = "1" };
            CommonOperatorExpressionMethods.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func);

            // Act
            var result = commonOperatorsExpressionBuilder.Build(filter, func);
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => (e.Id != 1)", expressionString);
            _MockRepository.VerifyAll();
        }


        [TestMethod]
        public void CommonOperatorsExpressionBuilder_Build_Filter_True_Works()
        {
            // Arrange
            var commonOperatorsExpressionBuilder = CreateCommonOperatorsExpressionBuilder();
            Filter<User> filter = new Filter<User> { Left = "1", Method = "eq", Right = "1" };
            CommonOperatorExpressionMethods.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func);

            // Act
            var result = commonOperatorsExpressionBuilder.Build(filter, func);
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => (1 == 1)", expressionString);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void CommonOperatorsExpressionBuilder_Build_Filter_False_Works()
        {
            // Arrange
            var commonOperatorsExpressionBuilder = CreateCommonOperatorsExpressionBuilder();
            Filter<User> filter = new Filter<User> { Left = "1", Method = "eq", Right = "0" };
            CommonOperatorExpressionMethods.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func);

            // Act
            var result = commonOperatorsExpressionBuilder.Build(filter, func);
            var expressionString = result.ToString();

            // Assert
            Assert.AreEqual("e => (1 == 0)", expressionString);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
