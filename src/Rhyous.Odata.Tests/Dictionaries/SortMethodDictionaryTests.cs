using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class SortMethodDictionaryTests
    {
        [TestMethod]
        public void OneToOneSortTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 };
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 };
            var entities = new List<User> { user1, user2 };

            var userType1 = new UserType { Id = 3, Name = "Example Users" };
            var relatedObjectJson = new JRaw(JsonConvert.SerializeObject(userType1.AsOdata<UserType, int>()));
            var relatedEntity1 = new RelatedEntity { Object = relatedObjectJson };
            var relatedEntities = new List<RelatedEntity> { relatedEntity1 };

            var sorterDictionary = new SortMethodDictionary<User>();
            var sortDetails = new SortDetails
            {
                EntityName = "User",
                RelatedEntity = "UserType",
                EntityToRelatedEntityProperty = "UserTypeId",
                RelatedEntityType = RelatedEntity.Type.OneToOne
            };

            // Act
            var actualCollections = sorterDictionary[RelatedEntity.Type.OneToOne](entities, relatedEntities, sortDetails);

            // Assert
            Assert.AreEqual(2, actualCollections.Count);

            Assert.AreEqual("User", actualCollections[0].Entity);
            Assert.AreEqual("UserType", actualCollections[0].RelatedEntity);
            Assert.AreEqual("1", actualCollections[0].EntityId);
            Assert.AreEqual(1, actualCollections[0].Entities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[0].Entities[0].Object);

            Assert.AreEqual("User", actualCollections[1].Entity);
            Assert.AreEqual("UserType", actualCollections[1].RelatedEntity);
            Assert.AreEqual("2", actualCollections[1].EntityId);
            Assert.AreEqual(1, actualCollections[1].Entities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[1].Entities[0].Object);
        }

        [TestMethod]
        public void ManyToManySortTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 }.AsOdata<User,int>();
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 }.AsOdata<User,int>();
            var user3 = new User { Id = 3, Name = "User3", UserTypeId = 4 }.AsOdata<User, int>();
            var entities = new List<OdataObject<User, int>> { user1, user2 };
            
            var userGroup1 = new UserGroup { Id = 1, Name = "Example Group 1" }.AsOdata<UserGroup,int>();
            var userGroup2 = new UserGroup { Id = 2, Name = "Example Group 2" }.AsOdata<UserGroup,int>();
            var userGroup3 = new UserGroup { Id = 3, Name = "Example Group 3" }.AsOdata<UserGroup,int>();
            var userGroup4 = new UserGroup { Id = 4, Name = "Example Group 4" }.AsOdata<UserGroup,int>();

            var userGroupMemberships = new List<OdataObject<UserGroupMembership, int>>
            {
                new UserGroupMembership{ Id = 2, UserGroupId = 4, UserId = 1 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 1, UserGroupId = 1, UserId = 1 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 4, UserGroupId = 2, UserId = 2 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 3, UserGroupId = 1, UserId = 2 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 5, UserGroupId = 3, UserId = 2 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 6, UserGroupId = 3, UserId = 3 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 7, UserGroupId = 4, UserId = 3 }.AsOdata<UserGroupMembership, int>(),
                new UserGroupMembership{ Id = 8, UserGroupId = 1, UserId = 3 }.AsOdata<UserGroupMembership, int>(),
            };

            var relatedEntityCollection = new RelatedEntityCollection { Entity = "UserGroupMembership", EntityId = "1", RelatedEntity = "UserGroup" };
            //relatedEntityCollection.Entities

            var relatedObjectJson = new JRaw(JsonConvert.SerializeObject(userGroupMemberships));
            var relatedEntity1 = new RelatedEntity { Object = relatedObjectJson };
            var relatedEntities = new List<RelatedEntity> { relatedEntity1 };

            var sorterDictionary = new SortMethodDictionary<OdataObject<User, int>>();
            var sortDetails = new SortDetails
            {
                EntityName = "User",
                RelatedEntity = "UserGroup",
                MappingEntity = "UserGroupMembership",
                EntityToRelatedEntityProperty = "UserTypeId",
                RelatedEntityType = RelatedEntity.Type.ManyToMany
            };

            // Act
            var actualCollections = sorterDictionary[RelatedEntity.Type.ManyToMany](entities, relatedEntities, sortDetails);

            // Assert
            Assert.AreEqual(2, actualCollections.Count);

            Assert.AreEqual("User", actualCollections[0].Entity);
            Assert.AreEqual("UserType", actualCollections[0].RelatedEntity);
            Assert.AreEqual("1", actualCollections[0].EntityId);
            Assert.AreEqual(1, actualCollections[0].Entities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[0].Entities[0].Object);

            Assert.AreEqual("User", actualCollections[1].Entity);
            Assert.AreEqual("UserType", actualCollections[1].RelatedEntity);
            Assert.AreEqual("2", actualCollections[1].EntityId);
            Assert.AreEqual(1, actualCollections[1].Entities.Count);
            Assert.AreEqual(relatedObjectJson, actualCollections[1].Entities[0].Object);
        }
    }
}