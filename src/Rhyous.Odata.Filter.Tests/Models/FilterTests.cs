using Microsoft.VisualStudio.TestTools.UnitTesting;

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
           Assert.IsFalse(filter.IsComplete);;
        }

        [TestMethod]
        public void FilterIsCompleteTrueTest()
        {
            // Arrange
            Filter<Entity1> filter = new Filter<Entity1> { Left = "Id", Right = "1", Method = "10" };

            // Act & Assert
            Assert.IsTrue(filter.IsComplete); ;
        }

        [TestMethod]
        public void FilterIsCompleteNonFilterTrueTest()
        {
            // Arrange
            Filter<Entity1> filter = "test";

            // Act & Assert
            Assert.IsTrue(filter.IsComplete); ;
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
    }
}
