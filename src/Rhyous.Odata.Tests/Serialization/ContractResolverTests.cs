using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rhyous.Odata.Tests
{
    [TestClass]
    public class ContractResolverTests
    {
        [TestMethod]
        public void ShouldSerializeNullList()
        {
            // Arrange
            List<int> list = null;

            // Act & Assert
            Assert.IsFalse(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(list));
        }

        [TestMethod]
        public void ShouldSerializeEmptyList()
        {
            // Arrange
            var list = new List<int>();

            // Act & Assert
            Assert.IsFalse(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(list));
        }

        [TestMethod]
        public void ShouldSerializeEmptyCollection()
        {
            // Arrange
            var list = new Collection<int>();

            // Act & Assert
            Assert.IsFalse(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(list));
        }
        
        [TestMethod]
        public void ShouldSerializeEmptyIEnumerable()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3};
            var enumerable = list.Where(i => i == 4);

            // Act & Assert
            Assert.IsFalse(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(enumerable));
        }

        [TestMethod]
        public void ShouldSerializePopulatedList()
        {
            // Arrange
            var list = new List<int> { 1, 2};

            // Act & Assert
            Assert.IsTrue(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(list));
        }

        [TestMethod]
        public void ShouldSerializePopulatedCollection()
        {
            // Arrange
            var list = new Collection<int> { 1, 2} ;

            // Act & Assert
            Assert.IsTrue(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(list));
        }

        [TestMethod]
        public void ShouldSerializePopulatedIEnumerable()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };
            var enumerable = list.Where(i => i < 4);

            // Act & Assert
            Assert.IsTrue(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(enumerable));
        }

        [TestMethod]
        public void ShouldSerializeEmptyRelatedEntityCollection()
        {
            // Arrange
            var odata = new OdataObject<Addendum, long>() { Object = new Addendum { Id = 10, Property = "A", Value = "B" } };

            // Act & Assert
            Assert.IsFalse(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(odata, new JsonProperty { PropertyName = "RelatedEntityCollection", UnderlyingName = "RelatedEntityCollection" }));
        }

        [TestMethod]
        public void ShouldSerializePropertyRenamedWithAttributeTest()
        {
            // Arrange
            var obj = new Smile { Id = 2, SmileType = "Big grin" };

            // Act & Assert
            Assert.IsTrue(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(obj, new JsonProperty { PropertyName = "Type", UnderlyingName = "SmileType" }));
        }

        [TestMethod]
        public void ShouldSerializePopulatedRelatedEntityCollection()
        {
            // Arrange
            var relatedEntityCollection = new RelatedEntityCollection { Entity = "Addendum", EntityId = "10", RelatedEntity = "Fake" };
            var json1 = "{ \"Id\" : \"1\" }";
            var jObject1 = JObject.Parse(json1);
            var json2 = "{ \"Id\" : \"2\" }";
            var jObject2 = JObject.Parse(json1);
            var relatedEntity1 = new RelatedEntity { Id = jObject1.GetValue("Id").ToString(), Object = new JRaw(json1) };
            var relatedEntity2 = new RelatedEntity { Id = jObject2.GetValue("Id").ToString(), Object = new JRaw(json2) };
            relatedEntityCollection.Add(relatedEntity1);
            relatedEntityCollection.Add(relatedEntity2);
            var odata = new OdataObject<Addendum, long>() { Object = new Addendum { Id = 10, Property = "A", Value = "B" } };
            odata.RelatedEntityCollection.Add(relatedEntityCollection);

            // Act & Assert
            Assert.IsTrue(ExcludeEmptyEnumerablesContractResolver.ShouldSerialize(odata, new JsonProperty { PropertyName = "RelatedEntityCollection", UnderlyingName = "RelatedEntityCollection" }));
        }
    }
}