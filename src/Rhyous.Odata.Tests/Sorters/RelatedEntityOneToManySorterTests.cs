using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class RelatedEntityOneToManySorterTests
    {
        [TestMethod]
        public void OneToManySortTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 };
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 };
            var entities = new List<User> { user1, user2 };

            var membership1 = new UserGroupMembership { Id = 11, UserId =  1, UserGroupId = 7 };
            var relatedObjectJson1 = new JRaw(JsonConvert.SerializeObject(membership1));
            var relatedEntity1 = new RelatedEntity { Object = relatedObjectJson1 };

            var membership2 = new UserGroupMembership { Id = 14, UserId = 2, UserGroupId = 8 };
            var relatedObjectJson2 = new JRaw(JsonConvert.SerializeObject(membership2));
            var relatedEntity2 = new RelatedEntity { Object = relatedObjectJson2 };

            var relatedEntities = new List<RelatedEntity> { relatedEntity1, relatedEntity2 };

            var sorterDictionary = new SortMethodDictionary<User>();
            var sortDetails = new SortDetails
            {
                EntityName = "User",
                RelatedEntity = "UserGroupMembership",
                EntityToRelatedEntityProperty = "UserId",
                RelatedEntityType = RelatedEntity.Type.OneToMany
            };

            // Act
            var actualCollections = sorterDictionary[RelatedEntity.Type.OneToMany](entities, relatedEntities, sortDetails);

            // Assert
            Assert.AreEqual(2, actualCollections.Count);

            Assert.AreEqual("User", actualCollections[0].Entity);
            Assert.AreEqual("UserGroupMembership", actualCollections[0].RelatedEntity);
            Assert.AreEqual("1", actualCollections[0].EntityId);
            Assert.AreEqual(1, actualCollections[0].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson1, actualCollections[0].RelatedEntities[0].Object);

            Assert.AreEqual("User", actualCollections[1].Entity);
            Assert.AreEqual("UserGroupMembership", actualCollections[1].RelatedEntity);
            Assert.AreEqual("2", actualCollections[1].EntityId);
            Assert.AreEqual(1, actualCollections[1].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson2, actualCollections[1].RelatedEntities[0].Object);
        }        
    }
}