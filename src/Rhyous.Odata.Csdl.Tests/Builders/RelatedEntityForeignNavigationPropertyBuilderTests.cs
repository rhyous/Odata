using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl.Tests.Builders
{
    [TestClass]
    public class RelatedEntityForeignNavigationPropertyBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomPropertyDataAppender> _MockCustomPropertyDataAppender;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomPropertyDataAppender = _MockRepository.Create<ICustomPropertyDataAppender>();
        }

        private RelatedEntityForeignNavigationPropertyBuilder CreateRelatedEntityForeignNavigationPropertyBuilder()
        {
            return new RelatedEntityForeignNavigationPropertyBuilder(_MockCustomPropertyDataAppender.Object);
        }

        #region Build
        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_NullAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = CreateRelatedEntityForeignNavigationPropertyBuilder();
            RelatedEntityForeignAttribute relatedEntityAttribute = null;

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ValidAttribute_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = CreateRelatedEntityForeignNavigationPropertyBuilder();
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1");
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity));

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result is CsdlNavigationProperty);
            Assert.AreEqual("self.Entity2", result.Type);
            Assert.AreEqual(CsdlConstants.NavigationProperty, result.Kind);
            Assert.IsTrue(result.IsCollection);
            Assert.IsTrue(result.Nullable);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ForeignKeyProperty_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = CreateRelatedEntityForeignNavigationPropertyBuilder();
            const string filter = "A eq 1";
            const string displayCondition = "B eq 2";
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1", "CustomProp") { Filter = filter, DisplayCondition = displayCondition };
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity));

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityForeignKeyProperty, out object prop));
            Assert.AreEqual(prop, "CustomProp");
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataFilter, out object odataFilter));
            Assert.AreEqual(odataFilter, filter);
            Assert.IsTrue(result.CustomData.TryGetValue(CsdlConstants.OdataDisplayCondition, out object odataDisplayCondition));
            Assert.AreEqual(odataDisplayCondition, displayCondition);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityForeignNavigationPropertyBuilder_Build_ForeignKeyPropertyDefault_Test()
        {
            // Arrange
            var funcs = new FuncList<string, string>();
            var unitUnderTest = CreateRelatedEntityForeignNavigationPropertyBuilder();
            var relatedEntityAttribute = new RelatedEntityForeignAttribute("Entity2", "Entity1", "Entity1Id");
            _MockCustomPropertyDataAppender.Setup(m => m.Append(It.IsAny<IConcurrentDictionary<string, object>>(), relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity));

            // Act
            var result = unitUnderTest.Build(relatedEntityAttribute);

            // Assert
            Assert.IsFalse(result.CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityForeignKeyProperty, out object prop));
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
