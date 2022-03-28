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

        #region GetHrefProperty
        [TestMethod]
        public void PropertyDataAttributeDictionary_GetHrefProperty_StringAsHtmlLink_Test()
        {
            // Arrange
            // Act
            var actual = new PropertyDataAttributeDictionary().GetHrefProperty(typeof(EntityWithHref).GetProperty(nameof(EntityWithHref.Link)));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(CsdlConstants.StringType, actual.First().Key);
            Assert.AreEqual(CsdlConstants.Href, actual.First().Value);
        }
        #endregion

        #region HandleCsdStringPropertyAttribute
        [TestMethod]
        public void PropertyDataAttributeDictionary_HandleCsdStringPropertyAttribute_StringType_Test()
        {
            // Arrange
            var dict = new PropertyDataAttributeDictionary();

            // Act
            var actual = dict.HandleCsdStringPropertyAttribute(typeof(EntityWithStringType).GetProperty(nameof(EntityWithStringType.Desciption)));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(CsdlConstants.StringType, actual.First().Key);
            Assert.AreEqual(CsdlConstants.TextArea, actual.First().Value);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_HandleCsdStringPropertyAttribute_StringType_InInterface_Test()
        {
            // Arrange
            var dict = new PropertyDataAttributeDictionary();

            // Act
            var actual = dict.HandleCsdStringPropertyAttribute(typeof(EntityWithStringTypeInInterface).GetProperty(nameof(EntityWithStringTypeInInterface.Desciption)));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(CsdlConstants.StringType, actual.First().Key);
            Assert.AreEqual(CsdlConstants.TextArea, actual.First().Value);
        }

        [TestMethod]
        public void PropertyDataAttributeDictionary_HandleCsdStringPropertyAttribute_StringType_InSubInterface_Test()
        {
            // Arrange
            var dict = new PropertyDataAttributeDictionary();

            // Act
            var actual = dict.HandleCsdStringPropertyAttribute(typeof(EntityWithStringTypeInSubInterface).GetProperty(nameof(EntityWithStringTypeInSubInterface.Desciption)));

            // Assert
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(CsdlConstants.StringType, actual.First().Key);
            Assert.AreEqual(CsdlConstants.TextArea, actual.First().Value);
        }
        #endregion
    }
}
