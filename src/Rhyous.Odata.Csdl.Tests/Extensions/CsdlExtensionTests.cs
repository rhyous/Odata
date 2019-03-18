using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Csdl.Tests
{
    [TestClass]
    public class CsdlExtensionTests
    {
        #region ToCsdl Type
        [TestMethod]
        public void CsdlExtensions_ToCsdl_TypeNull_Tests()
        {
            // Arrange
            Type type = null;

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.IsNull(csdl);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Valid_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(Person).ToCsdl();

            // Assert
            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);

            var expectedPropertyCount = typeof(Person).GetProperties().Length;
            expectedPropertyCount += typeof(Person).GetProperties().Where(p => p.GetCustomAttribute<RelatedEntityAttribute>() != null).Count();
            expectedPropertyCount += typeof(Person).GetCustomAttributes<RelatedEntityForeignAttribute>().Count();
            expectedPropertyCount += typeof(Person).GetCustomAttributes<RelatedEntityMappingAttribute>().Count();
            Assert.AreEqual(expectedPropertyCount, csdl.Properties.Count);

            Assert.IsTrue(csdl.Properties.TryGetValue("Id", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["Id"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("FirstName", out object _));
            Assert.AreEqual("Edm.String", (csdl.Properties["FirstName"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("LastName", out object _));
            Assert.AreEqual("Edm.String", (csdl.Properties["LastName"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("DateOfBirth", out object _));
            Assert.AreEqual("Edm.Date", (csdl.Properties["DateOfBirth"] as CsdlProperty).Type);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Enum_And_Double_Tests()
        {
            // Arrange
            // Act
            var csdl = typeof(SuiteMembership).ToCsdl();

            // Assert
            var expectedPropertyCount = typeof(SuiteMembership).GetProperties().Length;
            expectedPropertyCount += typeof(SuiteMembership).GetProperties().Where(p => p.GetCustomAttribute<RelatedEntityAttribute>() != null).Count();
            expectedPropertyCount += typeof(SuiteMembership).GetCustomAttributes<RelatedEntityForeignAttribute>().Count();
            expectedPropertyCount += typeof(SuiteMembership).GetCustomAttributes<RelatedEntityMappingAttribute>().Count();
            Assert.AreEqual(expectedPropertyCount, csdl.Properties.Count);

            Assert.AreEqual(1, csdl.Keys.Count);
            Assert.AreEqual("Id", csdl.Keys[0]);

            Assert.IsTrue(csdl.Properties.TryGetValue("Id", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["Id"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("SuiteId", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["SuiteId"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("ProductId", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["ProductId"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("Quantity", out object _));
            Assert.AreEqual("Edm.Double", (csdl.Properties["Quantity"] as CsdlProperty).Type);

            Assert.IsTrue(csdl.Properties.TryGetValue("QuantityType", out object _));
            Assert.AreEqual("Edm.Int32", (csdl.Properties["QuantityType"] as CsdlEnumProperty).UnderlyingType);

            foreach (var e in Enum.GetValues(typeof(QuantityType)))
            {
                Assert.AreEqual(e, (csdl.Properties["QuantityType"] as CsdlEnumProperty).CustomData[e.ToString()]);
            }

        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_EntityWithNullables_Tests()
        {
            // Arrange
            Type type = typeof(EntityWithNullables);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.IsFalse((csdl.Properties["Name"] as CsdlProperty).Nullable, "Name");
            Assert.IsTrue((csdl.Properties["Description"] as CsdlProperty).Nullable, "Description");
            Assert.IsFalse((csdl.Properties["CreateDate"] as CsdlProperty).Nullable, "CreateDate");
            Assert.IsFalse((csdl.Properties["CreatedByUserId"] as CsdlProperty).Nullable, "CreatedByUserId");
            Assert.IsTrue((csdl.Properties["LastUpdated"] as CsdlProperty).Nullable, "LastUpdated");
            Assert.IsTrue((csdl.Properties["LastUpdatedByUserId"] as CsdlProperty).Nullable, "LastUpdatedByUserId");
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_EntityWithFilter_Tests()
        {
            // Arrange
            Type type = typeof(EntityWithFilter);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.AreEqual("Type eq 1", (csdl.Properties["Type"] as CsdlNavigationProperty).CustomData["@Odata.Filter"]);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_TwoAttributesExists_OneWithAlias_Test()
        {
            // Arrange
            Type type = typeof(EntityWithDuplicateRelatedEntityOneAlias);

            // Act
            var csdl = type.ToCsdl();

            // Assert
            Assert.IsNotNull(csdl);
        }
        #endregion

        #region ToCsdl PropInfo
        [TestMethod]
        public void CsdlExtensions_ToCsdl_PropertyInfoNull_Tests()
        {
            // Arrange
            PropertyInfo propInfo = null;

            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.IsNull(csdl);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Property_Name_Test()
        {
            // Arrange
            PropertyInfo propInfo = typeof(Entity1).GetProperty("Name");

            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.AreEqual("Edm.String", csdl.Type);
            Assert.AreEqual(0, csdl.CustomData.Count);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Property_Name_CustomAttribute_Test()
        {
            // Arrange
            // Date has EditableAttribute set to false.
            PropertyInfo propInfo = typeof(Entity1).GetProperty("Date");

            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.AreEqual("Edm.Date", csdl.Type);
            Assert.AreEqual(1, csdl.CustomData.Count);
            Assert.AreEqual("@UI.ReadOnly", csdl.CustomData.First().Key);
            Assert.IsTrue((bool)csdl.CustomData.First().Value);
        }

        [TestMethod]
        public void CsdlExtensions_ToCsdl_Property_Name_CustomPropertyDictionary_Test()
        {
            // Arrange
            // Date has EditableAttribute set to false.
            PropertyInfo propInfo = typeof(Entity1).GetProperty("Date");


            // Act
            var csdl = propInfo.ToCsdl();

            // Assert
            Assert.AreEqual("Edm.Date", csdl.Type);
            Assert.AreEqual(1, csdl.CustomData.Count);
            Assert.AreEqual("@UI.ReadOnly", csdl.CustomData.First().Key);
            Assert.IsTrue((bool)csdl.CustomData.First().Value);
        }

        #endregion

        #region ToNavigationProperty


        [TestMethod]
        public void CsdlExtensions_ToCsdl_RelatedEntityMapping_WithAlias_Tests()
        {
            // Arrange
            //[RelatedEntityMapping()]
            var attribute = new RelatedEntityMappingAttribute("Product", "SuiteMembership", "Product") { EntityAlias = "Suite", MappingEntityAlias = "ProductMembership", RelatedEntityAlias = "ProductInSuite" };

            // Act
            var navProp = attribute.ToNavigationProperty();

            // Assert
            Assert.AreEqual("self.Product", navProp.Type);
            Assert.AreEqual("self.ProductInSuite", navProp.Alias);
        }
        #endregion
    }
}