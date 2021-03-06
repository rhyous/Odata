﻿using System;
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
        public void SettingObjectNoIdPropertyTest()
        {
            // Arrange
            string json = "{\"Key\":3,\"Addenda\":[],\"Object\":{\"Id\":3,\"UserGroupId\":3,\"UserId\":3},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/UserGroupMembershipService.svc/UserGroupMemberships(3)\"}";
            var relatedEntity = new RelatedEntity();

            // Act
            relatedEntity.Object = new JRaw(json);

            // Assert
            Assert.IsNull(relatedEntity.Id);
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

        [TestMethod]
        public void ImplicitOperatorNullTest()
        {
            // Arrange
            RelatedEntity<User,int> re1 = null;

            // Act
            RelatedEntity re2 = re1;

            // Assert
            Assert.IsNull(re2);
        }

        [TestMethod]
        public void ImplicitOperatorDefaultTest()
        {
            // Arrange
            var re1 = new RelatedEntity<User, int>();

            // Act
            RelatedEntity re2 = re1;

            // Assert
            Assert.IsNotNull(re2);
            Assert.AreEqual("0", re2.Id);
        }

        [TestMethod]
        public void ImplicitOperatorStringIdTest()
        {
            // Arrange
            var re1 = new RelatedEntity<Entity2, string>();

            // Act
            RelatedEntity re2 = re1;

            // Assert
            Assert.IsNotNull(re2);
            Assert.IsNull(re2.Id);
        }

        [TestMethod]
        public void ImplicitOperatorNewJRawStringTest()
        {
            // Arrange
            var re1 = new RelatedEntity<JRaw, string>();

            // Act
            RelatedEntity re2 = re1;

            // Assert
            Assert.IsNotNull(re2);
            Assert.IsNull(re2.Id);
        }
    }
}
