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
                Assert.IsTrue(validOperators.Contains(op.ToString()));
                Assert.IsTrue(validOperators.Contains(op.ToString().ToLower()));
            }
            Assert.IsTrue(validOperators.Contains(Conjunction.And.ToString()));
            Assert.IsTrue(validOperators.Contains(Conjunction.And.ToString().ToLower()));
            Assert.IsTrue(validOperators.Contains(Conjunction.Or.ToString()));
            Assert.IsTrue(validOperators.Contains(Conjunction.Or.ToString().ToLower()));
        }
        #endregion
    }
}
