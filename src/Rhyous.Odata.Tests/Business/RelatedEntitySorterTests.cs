using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Rhyous.Odata.Tests.Business
{
    [TestClass]
    public class RelatedEntitySorterTests
    {
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
    }
}
