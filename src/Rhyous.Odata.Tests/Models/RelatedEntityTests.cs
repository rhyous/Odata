using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class RelatedEntityTests
    {
        [TestMethod]
        public void SettingObjectSetsIdTest()
        {
            // Arrange
            string json = "{\"Id\":3,\"Addenda\":[],\"Object\":{\"Id\":3,\"UserGroupId\":3,\"UserId\":3},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}";
            var relatedEntity = new RelatedEntity();

            // Act
            relatedEntity.Object = new JRaw(json);

            // Assert
            Assert.AreEqual("3", relatedEntity.Id);
        }
    
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SettingObjectNoIdPropertyTest()
        {
            // Arrange
            string json = "{\"Key\":3,\"Addenda\":[],\"Object\":{\"Id\":3,\"UserGroupId\":3,\"UserId\":3},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}";
            var relatedEntity = new RelatedEntity();

            // Act
            relatedEntity.Object = new JRaw(json);

            // Assert - see Expected Exception
        }

        [TestMethod]
        public void SettingObjectKeyIdPropertyTest()
        {
            // Arrange
            string json = "{\"Key\":3,\"IdProperty\":\"Key\",\"Addenda\":[],\"Object\":{\"Id\":3,\"UserGroupId\":3,\"UserId\":3},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}";
            var relatedEntity = new RelatedEntity();

            // Act
            relatedEntity.Object = new JRaw(json);

            // Assert
            Assert.AreEqual("3", relatedEntity.Id);
        }
    }
}
