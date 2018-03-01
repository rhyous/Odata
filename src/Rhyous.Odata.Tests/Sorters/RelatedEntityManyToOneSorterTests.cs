using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class RelatedEntityManyToOneSorterTests
    {
        [TestMethod]
        public void ManyToOneSortTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 };
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 };
            var entities = new List<User> { user1, user2 };

            var userType1 = new UserType { Id = 3, Name = "Example Users" };
            var relatedObjectJson = new JRaw(JsonConvert.SerializeObject(userType1));
            var relatedEntity1 = new RelatedEntity { Object = relatedObjectJson };
            var relatedEntities = new List<RelatedEntity> { relatedEntity1 };

            var sorterDictionary = new SortMethodDictionary<User>();
            var sortDetails = new SortDetails
            {
                EntityName = "User",
                RelatedEntity = "UserType",
                EntityToRelatedEntityProperty = "UserTypeId",
                RelatedEntityType = RelatedEntity.Type.ManyToOne
            };

            // Act
            var actualCollections = sorterDictionary[RelatedEntity.Type.ManyToOne](entities, relatedEntities, sortDetails);

            // Assert
            Assert.AreEqual(2, actualCollections.Count);

            Assert.AreEqual("User", actualCollections[0].Entity);
            Assert.AreEqual("UserType", actualCollections[0].RelatedEntity);
            Assert.AreEqual("1", actualCollections[0].EntityId);
            Assert.AreEqual(1, actualCollections[0].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[0].RelatedEntities[0].Object);

            Assert.AreEqual("User", actualCollections[1].Entity);
            Assert.AreEqual("UserType", actualCollections[1].RelatedEntity);
            Assert.AreEqual("2", actualCollections[1].EntityId);
            Assert.AreEqual(1, actualCollections[1].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[1].RelatedEntities[0].Object);
        }

        [TestMethod]
        public void ManyToOneSortByPropertyOtherThanIdTest()
        {
            // Arrange
            var user1 = new User2 { Id = 1, Name = "User1", UserTypeName = "Example Users" };
            var user2 = new User2 { Id = 2, Name = "User2", UserTypeName = "Example Users" };
            var entities = new List<User2> { user1, user2 };

            var userType1 = new UserType { Id = 3, Name = "Example Users" };
            var relatedObjectJson = new JRaw(JsonConvert.SerializeObject(userType1));
            var relatedEntity1 = new RelatedEntity { Object = relatedObjectJson };
            var relatedEntities = new List<RelatedEntity> { relatedEntity1 };

            var sorterDictionary = new SortMethodDictionary<User2>();
            var sortDetails = new SortDetails
            {
                EntityName = "User",
                RelatedEntity = "UserType",
                EntityToRelatedEntityProperty = "UserTypeName",
                RelatedEntityType = RelatedEntity.Type.ManyToOne,
                RelatedEntityIdProperty = "Name"
            };

            // Act
            var actualCollections = sorterDictionary[RelatedEntity.Type.ManyToOne](entities, relatedEntities, sortDetails);

            // Assert
            Assert.AreEqual(2, actualCollections.Count);

            Assert.AreEqual("User", actualCollections[0].Entity);
            Assert.AreEqual("UserType", actualCollections[0].RelatedEntity);
            Assert.AreEqual("1", actualCollections[0].EntityId);
            Assert.AreEqual(1, actualCollections[0].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[0].RelatedEntities[0].Object);

            Assert.AreEqual("User", actualCollections[1].Entity);
            Assert.AreEqual("UserType", actualCollections[1].RelatedEntity);
            Assert.AreEqual("2", actualCollections[1].EntityId);
            Assert.AreEqual(1, actualCollections[1].RelatedEntities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[1].RelatedEntities[0].Object);
        }
    }
}