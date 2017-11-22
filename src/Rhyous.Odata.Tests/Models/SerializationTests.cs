using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Text;

namespace Rhyous.Odata.Tests.Models
{
    /// <summary>
    /// Just want to make sure that serialization is correct.
    /// </summary>
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void OdataObjectDataContractSerializerTests()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1" };
            var odataObject = user.AsOdata<User, int>();
            var expected = "{\"Uri\":null,\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"RelatedEntityCollection\":[],\"PropertyUris\":[]}";
            var serializer = new DataContractJsonSerializer(typeof(OdataObject<User, int>));

            // Act
            var json = Serialize(odataObject, serializer);

            // Assert
            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void OdataObjectDataContractSerializerDeserializeTest()
        {
            // Arrange
            var expected = "{\"Uri\":null,\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"RelatedEntityCollection\":[],\"PropertyUris\":[]}";
            var serializer = new DataContractJsonSerializer(typeof(OdataObject<User, int>));

            // Act
            var odataObjectUser = Deserialize<OdataObject<User, int>>(expected, serializer);

            // Assert
            Assert.AreEqual(1, odataObjectUser.Id);
            Assert.AreEqual(1, odataObjectUser.Object.Id);
            Assert.AreEqual("User1", odataObjectUser.Object.Name);
        }

        [TestMethod]
        public void OdataObjectJsonNetSerializerTests()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1" };
            var odataObject = user.AsOdata<User, int>();
            var expected = "{\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"Uri\":null}";
            var settings = new JsonSerializerSettings() { ContractResolver = new OrderedContractResolver() };
             
            // Act
            var json = JsonConvert.SerializeObject(odataObject, settings);

            // Assert
            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void OdataObjectJsonNetSerializerDeserializeTests()
        {
            // Arrange
            var json = "{\"Uri\":null,\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"RelatedEntities\":[],\"PropertyUris\":[]}";

            // Act
            var odataObjectUser = JsonConvert.DeserializeObject<OdataObject<User, int>>(json);

            // Assert
            Assert.AreEqual(1, odataObjectUser.Id);
            Assert.AreEqual(1, odataObjectUser.Object.Id);
            Assert.AreEqual("User1", odataObjectUser.Object.Name);
        }

        [TestMethod]
        public void OdataObjectCollectionDataContractSerializerTests()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1" };
            var user2 = new User { Id = 2, Name = "User2" };
            var user3 = new User { Id = 3, Name = "User3" };
            var odataObject1 = user1.AsOdata<User, int>();
            var odataObject2 = user2.AsOdata<User, int>();
            var odataObject3 = user3.AsOdata<User, int>();
            var collection = new OdataObjectCollection<User, int>();
            collection.AddRange(new[] { odataObject1, odataObject2, odataObject3 });
            var expected = "{\"Count\":3,\"Entities\":[{\"Uri\":null,\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"RelatedEntityCollection\":[],\"PropertyUris\":[]},{\"Uri\":null,\"Id\":2,\"Object\":{\"Id\":2,\"Name\":\"User2\",\"UserTypeId\":0},\"RelatedEntityCollection\":[],\"PropertyUris\":[]},{\"Uri\":null,\"Id\":3,\"Object\":{\"Id\":3,\"Name\":\"User3\",\"UserTypeId\":0},\"RelatedEntityCollection\":[],\"PropertyUris\":[]}],\"Entity\":\"User\",\"RelatedEntityCollection\":[]}";

            var serializer = new DataContractJsonSerializer(typeof(OdataObjectCollection<User, int>), new[] { typeof(OdataObject<User, int>) });

            // Act
            var json = Serialize(collection, serializer);

            // Assert
            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void OdataObjectCollectionDataContractSerializerDeserializeTests()
        {
            // Arrange
            var json = "{\"Count\":3,\"Entities\":[{\"Uri\":null,\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"RelatedEntities\":[],\"PropertyUris\":[]},{\"Uri\":null,\"Id\":2,\"Object\":{\"Id\":2,\"Name\":\"User2\",\"UserTypeId\":0},\"RelatedEntities\":[],\"PropertyUris\":[]},{\"Uri\":null,\"Id\":3,\"Object\":{\"Id\":3,\"Name\":\"User3\",\"UserTypeId\":0},\"RelatedEntities\":[],\"PropertyUris\":[]}],\"Entity\":\"User\",\"RelatedEntities\":[]}";

            var serializer = new DataContractJsonSerializer(typeof(OdataObjectCollection<User, int>), new[] { typeof(OdataObject<User, int>) });

            // Act
            var actual = Deserialize<OdataObjectCollection<User, int>>(json, serializer);

            // Assert
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(3, actual.Entities.Count);
            Assert.AreEqual(1, actual.Entities[0].Id);
            Assert.AreEqual(1, actual.Entities[0].Object.Id);
            Assert.AreEqual("User1", actual.Entities[0].Object.Name);
            Assert.AreEqual(2, actual.Entities[1].Id);
            Assert.AreEqual(2, actual.Entities[1].Object.Id);
            Assert.AreEqual("User2", actual.Entities[1].Object.Name);
            Assert.AreEqual(3, actual.Entities[2].Id);
            Assert.AreEqual(3, actual.Entities[2].Object.Id);
            Assert.AreEqual("User3", actual.Entities[2].Object.Name);
        }

        /// <summary>
        /// JSON serialization has the properties in slightly different order than DataContractJsonSerializer, but same data.
        /// </summary>
        [TestMethod]
        public void OdataObjectCollectionJsonNetSerializerTests()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1" };
            var user2 = new User { Id = 2, Name = "User2" };
            var user3 = new User { Id = 3, Name = "User3" };
            var odataObject1 = user1.AsOdata<User, int>();
            var odataObject2 = user2.AsOdata<User, int>();
            var odataObject3 = user3.AsOdata<User, int>();
            var expected = "{\"Count\":3,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"Name\":\"User1\",\"UserTypeId\":0},\"Uri\":null},{\"Id\":2,\"Object\":{\"Id\":2,\"Name\":\"User2\",\"UserTypeId\":0},\"Uri\":null},{\"Id\":3,\"Object\":{\"Id\":3,\"Name\":\"User3\",\"UserTypeId\":0},\"Uri\":null}],\"Entity\":\"User\"}";
            var collection = new OdataObjectCollection<User, int>();
            collection.AddRange(new[] { odataObject1, odataObject2, odataObject3 });            
            var settings = new JsonSerializerSettings() { ContractResolver = new OrderedContractResolver() };

            // Act
            var json = JsonConvert.SerializeObject(collection, settings);

            // Assert
            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void TestDeserialization()
        {
            // Arrange
            var json = "{\"Count\":1,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"UserGroupId\":1,\"UserId\":1},\"RelatedEntityCollection\":[{\"Count\":1,\"RelatedEntity\":\"UserGroup\",\"RelatedEntities\":[{\"Id\":\"1\",\"Object\":{\"Id\":1,\"Name\":\"System Users\",\"CreateDate\":\"2017-08-22T16:47:28.283\",\"CreatedBy\":1,\"Description\":null,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"Uri\":\"http://localhost:3896/UserGroupService.svc/UserGroups/Ids(1)\"}]}],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships/UserId/Values(1)\"}],\"Entity\":\"UserGroupMembership\"}";

            // Act
            var collection = JsonConvert.DeserializeObject<OdataObjectCollection>(json);

            // Assert
            Assert.AreEqual(1, collection.Entities.Count);
            Assert.AreEqual(1, collection.Entities[0].RelatedEntityCollection.Count);
        }

        #region Helper methods
        public static string Serialize<T>(T objectToSerialize, XmlObjectSerializer serializer)
        {
            using (MemoryStream memStm = new MemoryStream())
            {
                serializer.WriteObject(memStm, objectToSerialize);

                memStm.Seek(0, SeekOrigin.Begin);

                using (var streamReader = new StreamReader(memStm))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }

        public static T Deserialize<T>(string json, XmlObjectSerializer serializer)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T item = (T)serializer.ReadObject(ms);
            ms.Close();
            return item;
        }
        #endregion
    }
}
