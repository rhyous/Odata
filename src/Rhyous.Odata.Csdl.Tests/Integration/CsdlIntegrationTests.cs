using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using Rhyous.StringLibrary.Pluralization;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Models
{
    [TestClass]
    public class CsdlIntegrationTests
    {
        [TestMethod]
        public void Create_Csdl_Entity_NoRelations_Test()
        {
            // Arrange
            var type = typeof(Entity1);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.AreEqual("EntityType", csdl.Kind);
            CollectionAssert.AreEqual(new List<string> { "Id" }, csdl.Keys);
            Assert.IsFalse(csdl.HasStream);
            Assert.IsFalse(csdl.OpenType);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var propertyNames = properties.Select(p => p.Name).ToList();
            CollectionAssert.AreEquivalent(propertyNames, csdl.Properties.Keys.ToList());
        }

        [TestMethod]
        public void Create_Csdl_EntityWithRelatedEntity_Test()
        {
            // Arrange
            var type = typeof(A);

            // Act
            var csdl = type.ToCsdl();

            // Assert
         Assert.AreEqual("EntityType", csdl.Kind);
            CollectionAssert.AreEqual(new List<string> { "Id" }, csdl.Keys);
            Assert.IsFalse(csdl.HasStream);
            Assert.IsFalse(csdl.OpenType);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var propertyNames = properties.Select(p => p.Name).ToList();
            propertyNames.Add(nameof(B));
            CollectionAssert.AreEquivalent(propertyNames, csdl.Properties.Keys.ToList());
        }

        [TestMethod]
        public void Create_Csdl_EntityWithRelatedEntityForeign_Test()
        {
            // Arrange
            var type = typeof(B);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.AreEqual("EntityType", csdl.Kind);
            CollectionAssert.AreEqual(new List<string> { "Id" }, csdl.Keys);
            Assert.IsFalse(csdl.HasStream);
            Assert.IsFalse(csdl.OpenType);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var propertyNames = properties.Select(p => p.Name).ToList();
            propertyNames.Add(nameof(A).Pluralize());
            CollectionAssert.AreEquivalent(propertyNames, csdl.Properties.Keys.ToList());
        }

        [TestMethod]
        public void Create_Csdl_EntityWithRelatedEntity_Alias_Test()
        {
            // Arrange
            var type = typeof(C);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.AreEqual("EntityType", csdl.Kind);
            CollectionAssert.AreEqual(new List<string> { "Id" }, csdl.Keys);
            Assert.IsFalse(csdl.HasStream);
            Assert.IsFalse(csdl.OpenType);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var propertyNames = properties.Select(p => p.Name).ToList();
            propertyNames.Add("Y");
            CollectionAssert.AreEquivalent(propertyNames, csdl.Properties.Keys.ToList());
            var navProp = csdl.Properties["Y"] as CsdlNavigationProperty;
            Assert.AreEqual("self.Y", navProp.Alias);
            Assert.IsFalse(navProp.ContainsTarget);
            Assert.IsFalse(navProp.IsCollection);
            Assert.IsFalse(navProp.Nullable);
            Assert.IsNotNull(navProp.ReferentialConstraint);
            Assert.AreEqual(nameof(C.DId), navProp.ReferentialConstraint.LocalProperty);
            Assert.AreEqual(nameof(D.Id), navProp.ReferentialConstraint.ForeignProperty);
            Assert.AreEqual(1, navProp.ReferentialConstraint.CustomData.Count);
            Assert.AreEqual(nameof(D.Id), navProp.ReferentialConstraint.CustomData[nameof(C.DId)]);
            Assert.AreEqual("NavigationProperty", navProp.Kind);
            Assert.AreEqual("X", navProp.CustomData["@EAF.Entity.Alias"]);
            Assert.AreEqual("Local", navProp.CustomData["@EAF.RelatedEntity.Type"]);
        }

        [TestMethod]
        public void Create_Csdl_EntityWithRelatedEntityForeign_Alias_Test()
        {
            // Arrange
            var type = typeof(D);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.AreEqual("EntityType", csdl.Kind);
            CollectionAssert.AreEqual(new List<string> { "Id" }, csdl.Keys);
            Assert.IsFalse(csdl.HasStream);
            Assert.IsFalse(csdl.OpenType);
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var propertyNames = properties.Select(p => p.Name).ToList();
            var plural = "X".Pluralize();
            propertyNames.Add(plural);
            CollectionAssert.AreEquivalent(propertyNames, csdl.Properties.Keys.ToList());
            var navProp = csdl.Properties[plural] as CsdlNavigationProperty;
            Assert.AreEqual("self.X", navProp.Alias);
            Assert.IsFalse(navProp.ContainsTarget);
            Assert.IsTrue(navProp.IsCollection);
            Assert.IsTrue(navProp.Nullable);
            Assert.IsNull(navProp.ReferentialConstraint);
            Assert.AreEqual("NavigationProperty", navProp.Kind);
            Assert.AreEqual("Y", navProp.CustomData["@EAF.Entity.Alias"]);
            Assert.AreEqual("Foreign", navProp.CustomData["@EAF.RelatedEntity.Type"]);
        }
    }
}
