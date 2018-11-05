using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class PropertyDataAttributeDictionaryTests
    {

        #region GetReadOnlyProperty

        [TestMethod]
        public void PropertyDataAttributeDictionary_GetReadOnlyProperty_Null_Test()
        {
            // Arrange
            // Act
            var actual = PropertyDataAttributeDictionary.Instance.GetReadOnlyProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_GetReadOnlyProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = PropertyDataAttributeDictionary.Instance.GetReadOnlyProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_GetReadOnlyProperty_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = PropertyDataAttributeDictionary.Instance.GetReadOnlyProperty(typeof(Entity1).GetProperty("Date"));

            // Assert
            Assert.AreEqual("@UI.ReadOnly", actual.First().Key);
            Assert.IsTrue((bool)actual.First().Value);
        }
        #endregion

    }
}
