using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Dictionaries
{
    [TestClass]
    public class PropertyAttributeDictionaryTests
    {
        #region GetRelatedEntityProperties

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_Null_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyAttributeDictionary().GetRelatedEntityProperties(null);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_NoAttributes_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyAttributeDictionary().GetRelatedEntityProperties(typeof(Person));

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_AttributeExists_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyAttributeDictionary().GetRelatedEntityProperties(typeof(User).GetProperty("UserTypeId"));

            // Assert
            Assert.AreEqual("UserType", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_AttributeExistsWithAlias_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyAttributeDictionary().GetRelatedEntityProperties(typeof(EntityWithRelatedEntityAlias).GetProperty("Entity3Id"));

            // Assert
            Assert.AreEqual("E3", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.First().Value.GetType());
        }

        [TestMethod]
        public void PropertyAttributeDictionary_GetRelatedEntityProperties_TwoAttributesExists_OneWithAlias_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyAttributeDictionary().GetRelatedEntityProperties(typeof(EntityWithDuplicateRelatedEntityOneAlias).GetProperty("Entity3Id"));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual("E3", actual.First().Key);
            Assert.AreEqual(typeof(CsdlNavigationProperty), actual.FirstOrDefault().Value.GetType());
        }
        #endregion

    }
}
