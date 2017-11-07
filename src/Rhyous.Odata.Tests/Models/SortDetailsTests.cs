using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class SortDetailsTests
    {
        [TestMethod]
        public void SetRelatedEntityPropertyNotPresetTests()
        {
            // Arrange
            // Act
            var sortDetails = new SortDetails("User", "UserType", RelatedEntity.Type.OneToOne);

            // Assert
            Assert.AreEqual("UserTypeId", sortDetails.EntityToRelatedEntityProperty);
        }

        [TestMethod]
        public void SetRelatedEntityPropertyPresetTests()
        {
            // Arrange
            var sortDetails = new SortDetails();
            sortDetails.EntityToRelatedEntityProperty = "TypeId";
            // Act
            sortDetails.RelatedEntity = "UserType";

            // Assert
            Assert.AreEqual("TypeId", sortDetails.EntityToRelatedEntityProperty);
        }
    }
}
