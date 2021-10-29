using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.Odata.Tests.Models
{
    [TestClass]
    public class GroupTests
    {
        [TestMethod]
        public void ConstructorEmptyTest()
        {
            // Arrange
            // Act
            var group = new Group();

            // Assert
            Assert.IsNull(group.OpenChar);
            Assert.IsNull(group.CloseChar);
        }

        [TestMethod]
        public void ConstructorSingleCharTest()
        {
            // Arrange
            // Act
            var group = new Group('"');

            // Assert
            Assert.AreEqual('"', group.OpenChar);
            Assert.AreEqual('"', group.CloseChar);
        }

        [TestMethod]
        public void ConstructorSeparateOpenAndCloseCharTest()
        {
            // Arrange
            // Act
            var group = new Group('(', ')');

            // Assert
            Assert.AreEqual('(', group.OpenChar);
            Assert.AreEqual(')', group.CloseChar);
        }

        [TestMethod]
        public void OpenValidCharacterTest()
        {
            // Arrange
            var group = new Group('(', ')');

            // Act
            group.Open('(');

            // Assert
            Assert.IsTrue(group.IsOpen);
            Assert.AreEqual(group.WrapChar, '(');
        }

        [TestMethod]
        public void OpenInvalidCharacterTest()
        {
            // Arrange
            var group = new Group('(', ')');

            // Act & Assert
            Assert.ThrowsException<InvalidGroupingException>(() => { group.Open(')'); });
        }

        [TestMethod]
        public void CloseValidCharacterTest()
        {
            // Arrange
            var group = new Group('(', ')');
            group.Open('(');

            // Act
            group.Close(')');

            // Assert
            Assert.IsFalse(group.IsOpen);
            Assert.IsNull(group.WrapChar);
        }

        [TestMethod]
        public void CloseInvalidCharacterTest()
        {
            // Arrange
            var group = new Group('(', ')');
            group.Open('(');

            // Act & Assert
            Assert.ThrowsException<InvalidGroupingException>(() => { group.Close('('); });
        }

        [TestMethod]
        public void CloseUnopenedGroupTest()
        {
            // Arrange
            var group = new Group('(', ')');

            // Act & Assert
            Assert.ThrowsException<InvalidGroupingException>(() => { group.Close(')'); });
        }
    }
}