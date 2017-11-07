using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
    }
}
