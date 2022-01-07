using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using System;
using static Rhyous.Odata.Filter.Tests.FilterTests;

namespace Rhyous.Odata.Filter.Tests.Models
{
    [TestClass]
    public class ArrayFilterTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        private ArrayFilter<TEntity, TArrayItem> CreateArrayFilter<TEntity, TArrayItem>()
        {
            return new ArrayFilter<TEntity, TArrayItem>();
        }

        #region ToString
        [TestMethod]
        public void ArrayFilter_ToString_ArrayNull_Test()
        {
            // Arrange
            var arrayFilter = CreateArrayFilter<A, int>();

            // Act
            var result = arrayFilter.ToString();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ArrayFilter_ToString_ArrayPopulated_Test()
        {
            // Arrange
            var arrayFilter = CreateArrayFilter<A, int>();
            arrayFilter.Array = new[] { 10, 11, 12 };
            // Act
            var result = arrayFilter.ToString();

            // Assert
            Assert.AreEqual("(10,11,12)", result);
        }

        [TestMethod]
        public void ArrayFilter_ToString_Array_String_Populated_IncludesSpace_Test()
        {
            // Arrange
            var arrayFilter = CreateArrayFilter<A, string>();
            arrayFilter.Array = new[] { "Val1", "Val 2", "Val3" }; // Val 2 has a space

            // Act
            var result = arrayFilter.ToString();

            // Assert
            Assert.AreEqual("(Val1,'Val 2',Val3)", result);
        }

        [TestMethod]
        public void ArrayFilter_ToString_Array_String_Populated_IncludesSpaceAndSingleQuote_Test()
        {
            // Arrange
            var arrayFilter = CreateArrayFilter<A, string>();
            arrayFilter.Array = new[] { "Val1", "Val '2'", "Val3" }; // Val 2 has a space

            // Act
            var result = arrayFilter.ToString();

            // Assert
            Assert.AreEqual("(Val1,\"Val '2'\",Val3)", result);
        }
        #endregion
    }
}
