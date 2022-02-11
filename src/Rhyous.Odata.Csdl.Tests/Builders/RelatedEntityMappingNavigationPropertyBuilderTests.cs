using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class RelatedEntityMappingNavigationPropertyBuilderTests
    {
        private MockRepository _MockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


        }

        private RelatedEntityMappingNavigationPropertyBuilder CreateRelatedEntityMappingNavigationPropertyBuilder()
        {
            return new RelatedEntityMappingNavigationPropertyBuilder();
        }

        #region Build
        [TestMethod]
        public void RelatedEntityMappingNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var mockFuncEnumerable = new Mock<IFuncList<string, string>>();
            var unitUnderTest = CreateRelatedEntityMappingNavigationPropertyBuilder();
            RelatedEntityMappingAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RelatedEntityMappingNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var mockFuncEnumerable = new Mock<IFuncList<string, string>>();
            var unitUnderTest = CreateRelatedEntityMappingNavigationPropertyBuilder();
            var relatedEntityAttribute = new RelatedEntityMappingAttribute("Entity2", "Entity1To2Map", "Entity1");
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
        public void RelatedEntityMappingNavigationPropertyBuilder_Build_MappingEntityAlias_Test()
        {
            // Arrange
            var mockFuncEnumerable = new Mock<IFuncList<string, string>>();
            var unitUnderTest = CreateRelatedEntityMappingNavigationPropertyBuilder();
            const string filter = "A eq 1";
            const string displayCondition = "B eq 2";
            var relatedEntityAttribute = new RelatedEntityMappingAttribute("Entity2", "Entity1To2Map", "Entity1") { MappingEntityAlias = "E1E2Map", Filter = filter, DisplayCondition = displayCondition };
            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.EAFMappingEntityAlias, out object mappingEntityAlias));
            Assert.AreEqual(mappingEntityAlias, "E1E2Map");
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataFilter, out object odataFilter));
            Assert.AreEqual(odataFilter, filter);
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataDisplayCondition, out object odataDisplayCondition));
            Assert.AreEqual(odataDisplayCondition, displayCondition);
        }
        #endregion
    }
}
