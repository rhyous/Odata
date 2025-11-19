using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Rhyous.Odata.Filter.Tests.Models
{
    [TestClass]
    public class ValidOperatorsTests
    {
        private ValidOperators CreateValidOperators()
        {
            return new ValidOperators();
        }

        #region Constructor
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            // Act
            var validOperators = CreateValidOperators();

            // Assert
            foreach (var op in Enum.GetValues(typeof(Operator)))
            {
                Assert.Contains(op.ToString(), validOperators);
                Assert.Contains(op.ToString().ToLower(), validOperators);
            }
            Assert.Contains(Conjunction.And.ToString(), validOperators);
            Assert.Contains(Conjunction.And.ToString().ToLower(), validOperators);
            Assert.Contains(Conjunction.Or.ToString(), validOperators);
            Assert.Contains(Conjunction.Or.ToString().ToLower(), validOperators);
        }
        #endregion
    }
}
