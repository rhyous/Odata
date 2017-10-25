using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.Odata.Expand.Tests
{
    [TestClass]
    public class ExpandParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            // Arrange
            var paramValue = "User,Organization";
            var expected = new List<ExpandPath>
            {
                new ExpandPath { Entity = "User" } ,
                new ExpandPath { Entity = "Organization" }
            };

            // Act
            var actual = new ExpandParser().Parse(paramValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ExpandPathComparer());
        }

        [TestMethod]
        public void ParseSubExpandPathTest()
        {
            // Arrange
            var paramValue = "User/Organization";
            var expected = new List<ExpandPath>
            {
                new ExpandPath { Entity = "User", SubExpandPath = new ExpandPath { Entity = "Organization" } }
            };

            // Act
            var actual = new ExpandParser().Parse(paramValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ExpandPathComparer());
        }

        [TestMethod]
        public void ParseSubExpandPathAndParanthesisTest()
        {
            // Arrange
            var paramValue = "User($expand=Organization)";
            var expected = new List<ExpandPath>
            {
                new ExpandPath
                {
                    Entity = "User",
                    Parenthesis = "$expand=Organization"
                }
            };

            // Act
            var actual = new ExpandParser().Parse(paramValue);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ExpandPathComparer());
        }

        [TestMethod]
        public void ParseNameValueCollectionNullTest()
        {
            // Arrange
            NameValueCollection collection = null;

            // Act
            var actual = new ExpandParser().Parse(collection);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ParseNameValueCollectionNoExpandParamTest()
        {
            // Arrange
            var collection = new NameValueCollection();

            // Act
            var actual = new ExpandParser().Parse(collection);

            // Assert
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ParseNameValueCollectionTest()
        {
            // Arrange
            var collection = new NameValueCollection {
                { "$filter", "Id = 1" },
                { "$expand", "User,Organization" }
            };
            var expected = new List<ExpandPath>
            {
                new ExpandPath { Entity = "User" } ,
                new ExpandPath { Entity = "Organization" }
            };

            // Act
            var actual = new ExpandParser().Parse(collection);

            // Assert
            CollectionAssert.AreEqual(expected, actual, new ExpandPathComparer());
        }
    }
}