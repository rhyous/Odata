using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class OdataExtensionsTests
    {
        [TestMethod]
        public void EntityAsOdataTest()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1" };

            // Act
            var actual = user.AsOdata<User, int>();

            // Assert
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(user, actual.Object);
        }

        [TestMethod]
        public void EntityListAsOdataTest()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1" };
            var user2 = new User { Id = 2, Name = "User2" };
            var list = new List<User> { user1, user2 };

            // Act
            var actual = list.AsOdata<User, int>();

            // Assert
            Assert.AreEqual(1, actual[0].Id);
            Assert.AreEqual(user1, actual[0].Object);

            Assert.AreEqual(2, actual[1].Id);
            Assert.AreEqual(user2, actual[1].Object);
        }

        [TestMethod]
        public void EntitySetUrlTest()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1" };
            var expected = "/UserService.svc(1)";

            // Act
            var actual = user.AsOdata<User, int>("/UserService.svc");

            // Assert
            Assert.AreEqual(expected, actual.Uri.ToString());
        }

        [TestMethod]
        public void ToOdataObjectTest()
        {
            // Arrange
            var re = new RelatedEntity { Id = "7"};
            var smile = new Smile { Id = 7, SmileType = "Wide" };
            var json = JsonConvert.SerializeObject(smile);
            re.Object = new JRaw(json);

            // Act
            var actualObj = re.ToOdataObject<Smile, int>();

            // Assert
            Assert.AreEqual(7, actualObj.Id);
            Assert.AreEqual(7, actualObj.Object.Id);
            Assert.AreEqual("Wide", actualObj.Object.SmileType);
        }

        [TestMethod]
        public void GetRelatedEntity_Generic_Test()
        {
            // Arrange
            var odataUser = new OdataObject<User, int>();            
            var re1 = new RelatedEntity { Id = "7" };
            var smile1 = new Smile { Id = 7, SmileType = "Wide" };
            var json1 = JsonConvert.SerializeObject(smile1);
            re1.Object = new JRaw(json1);

            var re2 = new RelatedEntity { Id = "8" };
            var smile2 = new Smile { Id = 8, SmileType = "Flat" };
            var json2 = JsonConvert.SerializeObject(smile2);
            re2.Object = new JRaw(json2);
            var rec = new RelatedEntityCollection { re1, re2 };
            rec.RelatedEntity = "Smile";
            odataUser.RelatedEntityCollection.Add(rec);

            // Act
            var odataSmileCollection = odataUser.GetRelatedEntityCollection<Smile, int>();

            // Assert
            Assert.AreEqual(2, odataSmileCollection.Count);
            Assert.AreEqual(7, odataSmileCollection[0].Id);
            Assert.AreEqual("Wide", odataSmileCollection[0].Object.SmileType);
            Assert.AreEqual(8, odataSmileCollection[1].Id);
            Assert.AreEqual("Flat", odataSmileCollection[1].Object.SmileType);
        }

        [TestMethod]
        public void GetRelatedEntity_NotGeneric_Test()
        {
            // Arrange
            var odataUser = new OdataObject<User, int>();
            var re1 = new RelatedEntity { Id = "7" };
            var smile1 = new Smile { Id = 7, SmileType = "Wide" };
            var json1 = JsonConvert.SerializeObject(smile1);
            re1.Object = new JRaw(json1);

            var re2 = new RelatedEntity { Id = "8" };
            var smile2 = new Smile { Id = 8, SmileType = "Flat" };
            var json2 = JsonConvert.SerializeObject(smile2);
            re2.Object = new JRaw(json2);
            var rec = new RelatedEntityCollection { re1, re2 };
            rec.RelatedEntity = "Smile";
            odataUser.RelatedEntityCollection.Add(rec);

            // Act
            var odataSmileCollection = odataUser.GetRelatedEntityCollection(nameof(Smile));

            // Assert
            Assert.AreEqual(2, odataSmileCollection.Count);
            Assert.AreEqual("7", odataSmileCollection[0].Id);
            var jObj0 = JObject.Parse(odataSmileCollection[0].Object.ToString());
            Assert.AreEqual("Wide", jObj0["Type"]);
            Assert.AreEqual("8", odataSmileCollection[1].Id);
            var jObj1 = JObject.Parse(odataSmileCollection[1].Object.ToString());
            Assert.AreEqual("Flat", jObj1["Type"]);
        }

        [TestMethod]
        public void GetRelatedEntity_NotGeneric_NoRelatedEntities_Test()
        {
            // Arrange
            var odataUser = new OdataObject<User, int>();

            // Act
            var odataSmileCollection = odataUser.GetRelatedEntityCollection(nameof(Smile));

            // Assert
            Assert.IsNull(odataSmileCollection);
        }

        [TestMethod]
        public void GetRelatedEntity_NotGeneric_RelatedEntitiesDifferentEntity_Test()
        {
            // Arrange
            var odataUser = new OdataObject<User, int>();
            var re1 = new RelatedEntity { Id = "7" };
            var person1 = new Person { Id = 7, FirstName = "Jared" };
            var json1 = JsonConvert.SerializeObject(person1);
            re1.Object = new JRaw(json1);

            var rec = new RelatedEntityCollection { re1 };
            rec.RelatedEntity = "Person";
            odataUser.RelatedEntityCollection.Add(rec);

            // Act
            var odataPersonCollection = odataUser.GetRelatedEntityCollection(nameof(Smile));

            // Assert
            Assert.IsNull(odataPersonCollection);
        }
    }
}
