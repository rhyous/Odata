using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class RelatedEntityForeignNavigationPropertyBuilderTests
    {
        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            RelatedEntityForeignAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1");

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            Assert.IsTrue(result.IsCollection);
            Assert.IsTrue(result.Nullable);
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ForeignKeyProperty_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            const string filter = "A eq 1";
            const string displayCondition = "B eq 2";
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1", "CustomProp") { Filter = filter, DisplayCondition = displayCondition };

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityForeignKeyProperty, out object prop));
            Assert.AreEqual(prop, "CustomProp");
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataFilter, out object odataFilter));
            Assert.AreEqual(odataFilter, filter);
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataDisplayCondition, out object odataDisplayCondition));
            Assert.AreEqual(odataDisplayCondition, displayCondition);
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ForeignKeyPropertyDefault_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = new RelatedEntityForeignNavigationPropertyBuilder(funcs);
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1", "Entity1Id");

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsFalse(result.CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityForeignKeyProperty, out object prop));
        }
    }
}
