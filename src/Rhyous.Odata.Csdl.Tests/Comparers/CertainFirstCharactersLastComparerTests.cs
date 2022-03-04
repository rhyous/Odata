using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Rhyous.Odata.Csdl.Tests.Comparers
{
    [TestClass]
    public class CertainFirstCharactersLastComparerTests
    {
        private CertainFirstCharactersLastComparer CreateComparer(params char[] lastChars)
        {
            return new CertainFirstCharactersLastComparer(lastChars);
        }

        #region Compare
        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_Both_Null_0()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = null;
            string right = null;

            // Act
            var result = comparer.Compare(left,right);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftNull_RightEmpty_Minus1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = null;
            string right = "";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftEmpty_RightNull_1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "";
            string right = null;

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftEqualsRight_0()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "a";
            string right = "a";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftEmpty_RightPopulated_Minus1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "";
            string right = "A";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftPopulated_RightEmpty_1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "a";
            string right = "";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_FirstCharsEqual_NormalSort()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "@a";
            string right = "@b";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_NeitherStartsWithLastChar_NormalSort()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "a";
            string right = "b";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftStartsWithLastCharacter_RightDoesNot_Minus1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "@a";
            string right = "a";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void CertainFirstCharactersLastComparer_Compare_LeftDoesNotStartWithLastCharacter_RightDoes_1()
        {
            // Arrange
            var comparer = CreateComparer();
            string left = "a";
            string right = "@a";

            // Act
            var result = comparer.Compare(left, right);

            // Assert
            Assert.AreEqual(1, result);
        }
        #endregion
    }
}
