using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Rhyous.Odata.Tests.Serialization
{
    [TestClass]
    public class ParentOnDeserializationTests
    {
        [TestMethod]
        public void ParentIsSetAfterDeserialization()
        {
            // Arrange
            var json = "{\"Count\":1,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"UserId\":7247,\"UserRoleId\":1},\"RelatedEntityCollection\":[{\"Count\":1,\"RelatedEntity\":\"UserRole\",\"RelatedEntities\":[{\"Id\":\"1\",\"Object\":{\"Id\":1,\"Name\":\"Admin\",\"CreateDate\":\"2017-12-04T03:39:26.683\",\"CreatedBy\":2,\"Description\":\"A role to indicate that the user is an administrator\",\"Enabled\":true,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"Uri\":\"http://localhost:3896/UserRoleService.svc/UserRoles/Ids(1)\"}]}]}],\"Entity\":\"UserRoleMembership\"}";

            // Act
            var ooc = JsonConvert.DeserializeObject<OdataObjectCollection>(json);

            // Assert
            Assert.AreEqual(ooc, ooc[0].Parent);
        }

        [TestMethod]
        public void ParentIsSetAfterDeserializationAndCast()
        {
            // Arrange
            var json = "{\"Count\":1,\"Entities\":[{\"Id\":1,\"Object\":{\"Id\":1,\"UserId\":7247,\"UserRoleId\":1},\"RelatedEntityCollection\":[{\"Count\":1,\"RelatedEntity\":\"UserRole\",\"RelatedEntities\":[{\"Id\":\"1\",\"Object\":{\"Id\":1,\"Name\":\"Admin\",\"CreateDate\":\"2017-12-04T03:39:26.683\",\"CreatedBy\":2,\"Description\":\"A role to indicate that the user is an administrator\",\"Enabled\":true,\"LastUpdated\":null,\"LastUpdatedBy\":null},\"Uri\":\"http://localhost:3896/UserRoleService.svc/UserRoles/Ids(1)\"}]}]}],\"Entity\":\"UserRoleMembership\"}";
            var ooc = JsonConvert.DeserializeObject<OdataObjectCollection>(json);

            // Act
            RelatedEntityCollection rec = ooc;

            // Assert
            Assert.AreEqual(rec, rec.RelatedEntities.Parent);
            Assert.AreEqual(rec, rec[0].Parent);
        }
    }
}
