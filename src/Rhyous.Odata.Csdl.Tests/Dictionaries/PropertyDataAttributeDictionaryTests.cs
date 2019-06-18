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
            var actual = new PropertyDataAttributeDictionary().GetReadOnlyProperty(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_GetReadOnlyProperty_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyDataAttributeDictionary().GetReadOnlyProperty(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_GetReadOnlyProperty_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyDataAttributeDictionary().GetReadOnlyProperty(typeof(Entity1).GetProperty("Date"));

            // Assert
            Assert.AreEqual("@UI.ReadOnly", actual.First().Key);
            Assert.IsTrue((bool)actual.First().Value);
        }
        #endregion

        #region GetRelatedEntityPropertyData
        [TestMethod]
        public void PropertyDataAttributeDictionary_GetRelatedEntityPropertyData_TwoAttributesExists_OneWithAlias_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyDataAttributeDictionary().GetRelatedEntityPropertyData(typeof(EntityWithDuplicateRelatedEntityOneAlias).GetProperty("Entity3Id"));

            // Assert
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("$NavigationKey", actual.First().Key);
            Assert.AreEqual("E3", actual.First().Value);
        }
        #endregion
    }
}
