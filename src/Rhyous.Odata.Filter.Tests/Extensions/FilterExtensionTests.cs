using System;
using Rhyous.Odata.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Filter.Tests.Extensions
{
    [TestClass]
    public class FilterExtensionTests
    {
        #region ContainLeft
        [TestMethod]
        public void ContainLeftTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.ContainLeft(Conjunction.Or);

            // Assert
            Assert.AreEqual(container.Left, filter1);
            Assert.IsNotNull(container.Right);
        }

        [TestMethod]
        public void ContainLeftWithConjTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.ContainLeft(Conjunction.Or);

            // Assert
            Assert.AreEqual(container.Left, filter1);
            Assert.AreEqual("Or", container.Method);
            Assert.IsNotNull(container.Right);
        }

        #endregion

        #region ContainRight
        [TestMethod]
        public void ContainRightTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.ContainRight(Conjunction.Or);

            // Assert
            Assert.AreEqual(container.Right, filter1);
            Assert.AreEqual(container, filter1.Parent);
            Assert.IsNotNull(container.Left);
        }

        [TestMethod]
        public void ContainRightWithConjTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.ContainRight(Conjunction.Or);

            // Assert
            Assert.AreEqual(container.Right, filter1);
            Assert.AreEqual(container, filter1.Parent);
            Assert.AreEqual("Or", container.Method);
            Assert.IsNotNull(container.Left);
        }

        #endregion

        #region Contain
        [TestMethod]
        public void ContainOrTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.Contain(Conjunction.Or);

            // Assert
            Assert.AreEqual(filter1, container.Left);
            Assert.AreEqual(container, filter1.Parent);
            Assert.AreEqual("Or", container.Method);
            Assert.IsNotNull(container.Right);
        }

        [TestMethod]
        public void ContainAndTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";

            // Act
            var container = filter1.Contain(Conjunction.And);

            // Assert
            Assert.AreEqual(container.Left, filter1);
            Assert.AreEqual(container, filter1.Parent);
            Assert.AreEqual("And", container.Method);
            Assert.IsNotNull(container.Right);
        }

        [TestMethod]
        public void ContainAndThenOrTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";
            Filter<Entity1> filter2 = "Name == 'Jared Barneck'";
            var container1 = new Filter<Entity1> { Left = filter1, Method = "And", Right = filter2 };

            // Act
            var container2 = filter2.Contain(Conjunction.Or);

            // Assert
            Assert.AreEqual(container1, container2.Left);
            Assert.AreEqual(container2, container1.Parent);
            Assert.AreEqual("Or", container2.Method);
            Assert.IsNotNull(container2.Right);
        }

        [TestMethod]
        public void ContainOrThenAndTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";
            Filter<Entity1> filter2 = "Name == 'Jared Barneck'";
            var container1 = new Filter<Entity1> { Left = filter1, Method = "Or", Right = filter2 };

            // Act
            var container2 = filter2.Contain(Conjunction.And);

            // Assert
            Assert.AreEqual(container1.Right, container2);
            Assert.AreEqual(container1, container2.Parent);
            Assert.AreEqual("And", container2.Method);
            Assert.IsNotNull(container2.Right);
        }

        [TestMethod]
        public void ContainAndAndOrTests()
        {
            // Arrange
            Filter<Entity1> filter1 = "Id == 1";
            Filter<Entity1> filter2 = "Name == 'Jared Barneck'";
            Filter<Entity1> filter3 = "Name == 'Jared A. Barneck'";
            var container1 = new Filter<Entity1> { Left = filter1, Method = "And", Right = filter2 };
            var container2 = new Filter<Entity1> { Left = container1, Method = "And", Right = filter3 };

            // Act
            var container3 = filter3.Contain(Conjunction.Or);

            // Assert
            Assert.AreEqual(container2, container3.Left);
            Assert.AreEqual(container3, container2.Parent);
            Assert.AreEqual("Or", container3.Method);
            Assert.IsNotNull(container3.Right);
        }
        #endregion
    }
}
