using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Rhyous.Odata.Tests.Models
{
    [TestClass]
    public class RelatedEntityOneToManyTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var json = "{\"Count\":2,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"7\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop1\",\"Value\":\"val1\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(1)\"},{\"Id\":2,\"Object\":{\"Id\":2,\"CreateDate\":\"2017-11-09T13:36:25.263\",\"CreatedBy\":1,\"Entity\":\"User\",\"EntityId\":\"7\",\"LastUpdated\":null,\"LastUpdatedBy\":null,\"Property\":\"Prop2\",\"Value\":\"val2\"},\"PropertyUris\":[],\"RelatedEntities\":[],\"Uri\":\"http://localhost:3896/AddendumService.svc/Addenda(2)\"}],\"Entity\":\"Addendum\",\"RelatedEntities\":[]}";
            var objs = JsonConvert.DeserializeObject<OdataObjectCollection>(json);

            // Act
            var list = new List<RelatedEntityOneToMany>();
            foreach (var e in objs)
            {
                var re = new RelatedEntityOneToMany("EntityId", e);
                list.Add(re);
            }

            // Assert
            Assert.AreEqual("7", list[0].RelatedId);
            Assert.AreEqual("7", list[1].RelatedId);
        }
    }
}
