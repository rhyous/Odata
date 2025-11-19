using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Rhyous.Odata.Tests;
using System.Linq;

namespace Rhyous.Odata.Expand.Tests
{
    [TestClass]
    public class AttributeEvaluatorTests
    {
        [TestMethod]
        public void GetAttributesToExpandNullNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var evaluator = new AttributeEvaluator();

            // Act
            var actual = evaluator.GetAttributesToExpand<RelatedEntityAttribute>(typeof(User), entitiesToExpand).ToList();

            // Assert
            Assert.IsEmpty(actual);
        }

        [TestMethod]
        public void GetAttributesToExpandNullAutoexpandTrueTest()
        {
            // Arrange
            List<string> entitiesToExpand = null;
            var evaluator = new AttributeEvaluator();
            var expected = new RelatedEntityAttribute("User") { Property = "UserId", AutoExpand = true };

            // Act
            var actual = evaluator.GetAttributesToExpand<RelatedEntityAttribute>(typeof(Token), entitiesToExpand).ToList();

            // Assert
            Assert.HasCount(1, actual);
            foreach (var prop in typeof(RelatedEntityAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }

        [TestMethod]
        public void GetAttributesToExpandUserTypeNoAutoexpandTest()
        {
            // Arrange
            List<string> entitiesToExpand = new List<string> { "UserType" };
            var evaluator = new AttributeEvaluator();
            var expected = new RelatedEntityAttribute("UserType") { Property = "UserTypeId" };

            // Act
            var actual = evaluator.GetAttributesToExpand<RelatedEntityAttribute>(typeof(User), entitiesToExpand).ToList();

            // Assert
            Assert.HasCount(1, actual);
            foreach (var prop in typeof(RelatedEntityAttribute).GetProperties())
            {
                Assert.AreEqual(prop.GetValue(expected), prop.GetValue(actual[0]));
            }
        }
    }
}