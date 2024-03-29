using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Extensions
{
    [TestClass]
    public class RelatedEntityAttributeExtensionsTests
    {
        #region Merge
        [TestMethod]
        public void RelatedEntityAttribute_Merge_IGrouping_ExpectedBehavior()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2");
            var re2 = new RelatedEntityAttribute("Entity2");
            IEnumerable<RelatedEntityAttribute> relatedEntityAttribs = new[] { re1, re2};
            var group = relatedEntityAttribs.GroupBy(a => a.Entity).FirstOrDefault();

            // Act
            var result = group.Merge();

            // Assert
            Assert.IsTrue(result is RelatedEntityAttribute);
        }

        [TestMethod]
        public void RelatedEntityAttribute_Merge_TwoAttribs_Test()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2");
            var re2 = new RelatedEntityAttribute("Entity2");

            // Act
            var result = RelatedEntityAttributeExtensions.Merge(re1, re2);

            // Assert
            Assert.IsTrue(result is RelatedEntityAttribute);
        }

        [TestMethod]
        public void RelatedEntityAttribute_Merge_TwoAttribs_DifferentEntities_Test()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2") { Entity = "E3" };
            var re2 = new RelatedEntityAttribute("Entity2") { Entity = "E4" };

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                RelatedEntityAttributeExtensions.Merge(re1, re2);
            });
        }

        [TestMethod]
        public void RelatedEntityAttribute_Merge_TwoAttribs_DifferentEntityProperties_Test()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2") { Entity = "E3", Property = "P1" };
            var re2 = new RelatedEntityAttribute("Entity2") { Entity = "E3", Property = "P2" };

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                RelatedEntityAttributeExtensions.Merge(re1, re2);
            });
        }


        [TestMethod]
        public void RelatedEntityAttribute_Merge_TwoAttribs_LeftPopulated_Test()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2")
            {
                EntityAlias = "Alias1",
                AutoExpand = true,
                Filter = "TypId eq 1",
                ForeignKeyProperty = "Name",
                ForeignKeyType = typeof(string),
                Property = "Entity2Name",
                GetAll = false,
                Nullable = true,
                RelatedEntityAlias = "E2",
                RelatedEntityMustExist = false,
                Entity = "Entity1"
            };
            var re2 = new RelatedEntityAttribute("Entity2")
            {
                Entity = "Entity1",
                Property = "Entity2Name"
            };

            // Act
            var result = RelatedEntityAttributeExtensions.Merge(re1, re2);

            // Assert
            Assert.AreEqual("Alias1", result.EntityAlias, "RelatedEntityAlias");
            Assert.IsTrue(result.AutoExpand, "AutoExpand");
            Assert.AreEqual("TypId eq 1", result.Filter, "Filter");
            Assert.AreEqual("Name", result.ForeignKeyProperty, "ForeignKeyProperty");
            Assert.AreEqual(typeof(string), result.ForeignKeyType, "ForeignKeyType");
            Assert.AreEqual("Entity2Name", result.Property, "Property");
            Assert.IsFalse(result.GetAll, "GetAll");
            Assert.IsTrue(result.Nullable, "Nullable");
            Assert.AreEqual("E2", result.RelatedEntityAlias, "RelatedEntityAlias");
            Assert.IsFalse(result.RelatedEntityMustExist, "RelatedEntityMustExist");
            Assert.AreEqual("Entity1", result.Entity, "Entity");
        }

        [TestMethod]
        public void RelatedEntityAttribute_Merge_TwoAttribs_RightPopulated_Test()
        {
            // Arrange
            var re1 = new RelatedEntityAttribute("Entity2")
            {
                EntityAlias = "Alias1",
                AutoExpand = true,
                Filter = "TypId eq 1",
                ForeignKeyProperty = "Name",
                ForeignKeyType = typeof(string),
                Property = "Entity2Name",
                GetAll = true,
                Nullable = true,
                RelatedEntityAlias = "E2",
                RelatedEntityMustExist = false,
                Entity = "Entity1"
            };
            var re2 = new RelatedEntityAttribute("Entity2")
            {
                Entity = "Entity1",
                Property = "Entity2Name"
            };

            // Act
            var result = RelatedEntityAttributeExtensions.Merge(re2, re1);

            // Assert
            Assert.AreEqual("Alias1", result.EntityAlias, "RelatedEntityAlias");
            Assert.IsTrue(result.AutoExpand, "AutoExpand");
            Assert.AreEqual("TypId eq 1", result.Filter, "Filter");
            Assert.AreEqual("Name", result.ForeignKeyProperty, "ForeignKeyProperty");
            Assert.AreEqual(typeof(string), result.ForeignKeyType, "ForeignKeyType");
            Assert.AreEqual("Entity2Name", result.Property, "Property");
            Assert.IsTrue(result.GetAll, "GetAll");
            Assert.IsTrue(result.Nullable, "Nullable");
            Assert.AreEqual("E2", result.RelatedEntityAlias, "RelatedEntityAlias");
            Assert.IsFalse(result.RelatedEntityMustExist, "RelatedEntityMustExist");
            Assert.AreEqual("Entity1", result.Entity, "Entity");
        }
        #endregion

        #region GetValue
        [TestMethod]
        public void RelatedEntityAttribute_GetValue_BothEmpty_Test()
        {
            // Arrange
            string l = "";
            string r = "";

            // Act
            var actual = RelatedEntityAttributeExtensions.GetValue(l, r, CsdlConstants.Id, string.IsNullOrWhiteSpace);

            // Assert
            Assert.AreEqual(CsdlConstants.Id, actual);
        }


        [TestMethod]
        public void RelatedEntityAttribute_GetValue_LeftEmpty_Test()
        {
            // Arrange
            string l = "";
            string r = "A";

            // Act
            var actual = RelatedEntityAttributeExtensions.GetValue(l, r, "x", string.IsNullOrWhiteSpace);

            // Assert
            Assert.AreEqual("A", actual);
        }

        [TestMethod]
        public void RelatedEntityAttribute_GetValue_RightEmtpy_Test()
        {
            // Arrange
            string l = "A";
            string r = "";

            // Act
            var actual = RelatedEntityAttributeExtensions.GetValue(l, r, "x");

            // Assert
            Assert.AreEqual("A", actual);
        }

        [TestMethod]
        public void RelatedEntityAttribute_GetValue_LeftId_Test()
        {
            // Arrange
            string l = "Id";
            string r = "OtherId";

            // Act
            var actual = RelatedEntityAttributeExtensions.GetValue(l, r, l, string.IsNullOrWhiteSpace);

            // Assert
            Assert.AreEqual("OtherId", actual);
        }

        [TestMethod]
        public void RelatedEntityAttribute_GetValue_RightId_Test()
        {
            // Arrange
            string l = "OtherId";
            string r = "Id";

            // Act
            var actual = RelatedEntityAttributeExtensions.GetValue(l, r, "x");

            // Assert
            Assert.AreEqual("OtherId", actual);
        }
        #endregion
    }
}