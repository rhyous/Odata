using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Rhyous.Odata.Tests.Business
{
    [TestClass]
    public class RelatedEntitySorterTests
    {
        #region Sort
        [TestMethod]
        public void SortTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 };
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 };
            var user3 = new User { Id = 3, Name = "User3", UserTypeId = 4 };
            var users = new [ ] { user1, user2, user3 };

            var userType1 = new UserType { Id = 3, Name = "Example Users" };
            var userType2 = new UserType { Id = 4, Name = "Example Users" };
            var userType1Json = new JRaw(JsonConvert.SerializeObject(userType1.AsOdata<UserType, int>()));
            var userType2Json = new JRaw(JsonConvert.SerializeObject(userType2.AsOdata<UserType, int>()));

            var relatedEntity1 = new RelatedEntity { Object = userType1Json };
            var relatedEntity2 = new RelatedEntity { Object = userType1Json };
            var relatedEntity3 = new RelatedEntity { Object = userType2Json };
            var relatedEntitites = new[] { relatedEntity1 , relatedEntity2, relatedEntity3 };
            
            var sorter = new RelatedEntitySorter<User, int>();
            var sortDetails = new SortDetails("User", "UserType", RelatedEntity.Type.ManyToOne);

            // Act
            var collection = sorter.Sort(users, relatedEntitites, sortDetails);

            // Assert
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual("User", collection[0].Entity);
            Assert.AreEqual("UserType", collection[0].RelatedEntity);
            Assert.AreEqual("1", collection[0].EntityId);
            Assert.AreEqual(2, collection[0].RelatedEntities.Count);

            Assert.AreEqual("User", collection[1].Entity);
            Assert.AreEqual("UserType", collection[1].RelatedEntity);
            Assert.AreEqual("2", collection[1].EntityId);
            Assert.AreEqual(2, collection[1].RelatedEntities.Count);

            Assert.AreEqual("User", collection[2].Entity);
            Assert.AreEqual("UserType", collection[2].RelatedEntity);
            Assert.AreEqual("3", collection[2].EntityId);
            Assert.AreEqual(1, collection[2].RelatedEntities.Count);
        }

        [TestMethod]
        public void SortByPropertyOtherThanIdTest()
        {
            // Arrange
            var user1 = new User2 { Id = 1, Name = "User1", UserTypeName = "Example Users 3" };
            var user2 = new User2 { Id = 2, Name = "User2", UserTypeName = "Example Users 3" };
            var user3 = new User2 { Id = 3, Name = "User3", UserTypeName = "Example Users 4" };
            var users = new[] { user1, user2, user3 };

            var userType1 = new UserType { Id = 3, Name = "Example Users 3" };
            var userType2 = new UserType { Id = 4, Name = "Example Users 4" };
            var userType1Json = new JRaw(JsonConvert.SerializeObject(userType1.AsOdata<UserType, int>()));
            var userType2Json = new JRaw(JsonConvert.SerializeObject(userType2.AsOdata<UserType, int>()));

            var relatedEntity1 = new RelatedEntity { Object = userType1Json };
            var relatedEntity2 = new RelatedEntity { Object = userType1Json };
            var relatedEntity3 = new RelatedEntity { Object = userType2Json };
            var relatedEntitites = new[] { relatedEntity1, relatedEntity2, relatedEntity3 };

            var sorter = new RelatedEntitySorter<User2, int>();
            var sortDetails = new SortDetails("User", "UserType", RelatedEntity.Type.ManyToOne)
            {
                RelatedEntityIdProperty = "Name",
                EntityToRelatedEntityProperty = "UserTypeName"
            };

            // Act
            var collection = sorter.Sort(users, relatedEntitites, sortDetails);

            // Assert
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual("User", collection[0].Entity);
            Assert.AreEqual("UserType", collection[0].RelatedEntity);
            Assert.AreEqual("1", collection[0].EntityId);
            Assert.AreEqual(2, collection[0].RelatedEntities.Count);

            Assert.AreEqual("User", collection[1].Entity);
            Assert.AreEqual("UserType", collection[1].RelatedEntity);
            Assert.AreEqual("2", collection[1].EntityId);
            Assert.AreEqual(2, collection[1].RelatedEntities.Count);

            Assert.AreEqual("User", collection[2].Entity);
            Assert.AreEqual("UserType", collection[2].RelatedEntity);
            Assert.AreEqual("3", collection[2].EntityId);
            Assert.AreEqual(1, collection[2].RelatedEntities.Count);
        }
        #endregion

        #region Collate
        [TestMethod]
        public void CollateTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1", UserTypeId = 3 }.AsOdata<User, int>();
            var user2 = new User { Id = 2, Name = "User2", UserTypeId = 3 }.AsOdata<User, int>();
            var user3 = new User { Id = 3, Name = "User3", UserTypeId = 4 }.AsOdata<User, int>();
            var entities = new List<OdataObject<User, int>> { user1, user2, user3 };

            var userType1 = new UserType { Id = 3, Name = "Example Users" };
            var userType2 = new UserType { Id = 4, Name = "Example Users" };
            var userType1Json = new JRaw(JsonConvert.SerializeObject(userType1.AsOdata<UserType, int>()));
            var userType2Json = new JRaw(JsonConvert.SerializeObject(userType2.AsOdata<UserType, int>()));

            var relatedEntity1 = new RelatedEntity { Object = userType1Json };
            var relatedEntity2 = new RelatedEntity { Object = userType1Json };
            var relatedEntity3 = new RelatedEntity { Object = userType2Json };

            var collection1 = new RelatedEntityCollection { Entity = "User", EntityId = "1", RelatedEntity = "UserType" };
            collection1.RelatedEntities.Add(relatedEntity1);

            var collection2 = new RelatedEntityCollection { Entity = "User", EntityId = "2", RelatedEntity = "UserType" };
            collection2.RelatedEntities.Add(relatedEntity2);

            var collection3 = new RelatedEntityCollection { Entity = "User", EntityId = "3", RelatedEntity = "UserType" };
            collection3.RelatedEntities.Add(relatedEntity3);

            var collections = new List<RelatedEntityCollection> { collection1, collection2, collection3 };

            var sorter = new RelatedEntitySorter<User, int>();

            // Act
            sorter.Collate(entities, collections);

            // Assert
            Assert.AreEqual(1, user1.RelatedEntityCollection.Count);
            Assert.AreEqual(user1.RelatedEntityCollection[0], collection1);

            Assert.AreEqual(1, user2.RelatedEntityCollection.Count);
            Assert.AreEqual(user2.RelatedEntityCollection[0], collection2);

            Assert.AreEqual(1, user3.RelatedEntityCollection.Count);
            Assert.AreEqual(user3.RelatedEntityCollection[0], collection3);
        }

        [TestMethod]
        public void CollatePropertyOtherThanIdTest()
        {
            // Arrange
            var user1 = new User2 { Id = 1, Name = "User1", UserTypeName = "Example Users1" }.AsOdata<User2, int>();
            var user2 = new User2 { Id = 2, Name = "User2", UserTypeName = "Example Users1" }.AsOdata<User2, int>();
            var user3 = new User2 { Id = 3, Name = "User3", UserTypeName = "Example Users2" }.AsOdata<User2, int>();
            var entities = new List<OdataObject<User2, int>> { user1, user2, user3 };

            var userType1 = new UserType { Id = 3, Name = "Example Users1" };
            var userType2 = new UserType { Id = 4, Name = "Example Users2" };
            var userType1Json = new JRaw(JsonConvert.SerializeObject(userType1.AsOdata<UserType, int>()));
            var userType2Json = new JRaw(JsonConvert.SerializeObject(userType2.AsOdata<UserType, int>()));

            var relatedEntity1 = new RelatedEntity { Object = userType1Json, Id = userType1.Name, IdProperty = "Name" };
            var relatedEntity2 = new RelatedEntity { Object = userType1Json, Id = userType1.Name, IdProperty = "Name" };
            var relatedEntity3 = new RelatedEntity { Object = userType2Json, Id = userType2.Name, IdProperty = "Name" };

            var collection1 = new RelatedEntityCollection { Entity = "User", EntityId = "1", RelatedEntity = "UserType" };
            collection1.RelatedEntities.Add(relatedEntity1);

            var collection2 = new RelatedEntityCollection { Entity = "User", EntityId = "2", RelatedEntity = "UserType" };
            collection2.RelatedEntities.Add(relatedEntity2);

            var collection3 = new RelatedEntityCollection { Entity = "User", EntityId = "3", RelatedEntity = "UserType" };
            collection3.RelatedEntities.Add(relatedEntity3);

            var collections = new List<RelatedEntityCollection> { collection1, collection2, collection3 };

            var sorter = new RelatedEntitySorter<User2, int>();

            // Act
            sorter.Collate(entities, collections);

            // Assert
            Assert.AreEqual(1, user1.RelatedEntityCollection.Count);
            Assert.AreEqual(user1.RelatedEntityCollection[0], collection1);

            Assert.AreEqual(1, user2.RelatedEntityCollection.Count);
            Assert.AreEqual(user2.RelatedEntityCollection[0], collection2);

            Assert.AreEqual(1, user3.RelatedEntityCollection.Count);
            Assert.AreEqual(user3.RelatedEntityCollection[0], collection3);
        }
        #endregion Collate

    }
}
