using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Tests.Converters
{
    [TestClass]
    public class RelatedEntityConverterTests
    {
        private RelatedEntityConverter _converter;

        [TestInitialize]
        public void TestInitialize()
        {
            _converter = new RelatedEntityConverter();
        }

        #region Convert<TEntity, TId>(RelatedEntity) - Generic Single Object

        [TestMethod]
        public void Convert_Generic_SingleObject_WithAllProperties_Test()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1", UserTypeId = 5 };
            var json = JsonConvert.SerializeObject(user);
            var relatedEntity = new RelatedEntity
            {
                Id = "1",
                IdProperty = "Id",
                Uri = new Uri("/UserService.svc(1)", UriKind.Relative),
                Object = new JRaw(json),
                PropertyUris = new List<OdataUri> { new OdataUri { PropertyName = "Name", Uri = new Uri("/UserService.svc(1)/Name", UriKind.Relative) } },
                RelatedEntityCollection = new Rhyous.Collections.ParentedList<RelatedEntityCollection>()
            };

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Id", result.IdProperty);
            Assert.AreEqual("/UserService.svc(1)", result.Uri.ToString());
            Assert.IsNotNull(result.Object);
            Assert.AreEqual(1, result.Object.Id);
            Assert.AreEqual("User1", result.Object.Name);
            Assert.AreEqual(5, result.Object.UserTypeId);
            Assert.IsNotNull(result.PropertyUris);
            Assert.AreEqual(1, result.PropertyUris.Count);
            Assert.IsNotNull(result.RelatedEntityCollection);
        }

        [TestMethod]
        public void Convert_Generic_SingleObject_NullInput_ReturnsNull_Test()
        {
            // Arrange
            RelatedEntity relatedEntity = null;

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_Generic_SingleObject_NullId_Test()
        {
            // Arrange
            var user = new User { Id = 0, Name = "User1" };
            var json = JsonConvert.SerializeObject(user);
            var relatedEntity = new RelatedEntity
            {
                Id = null,
                Object = new JRaw(json)
            };

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(default(int), result.Id);
        }

        [TestMethod]
        public void Convert_Generic_SingleObject_EmptyId_Test()
        {
            // Arrange
            var user = new User { Id = 0, Name = "User1" };
            var json = JsonConvert.SerializeObject(user);
            var relatedEntity = new RelatedEntity
            {
                Id = "",
                Object = new JRaw(json)
            };

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(default(int), result.Id);
        }

        [TestMethod]
        public void Convert_Generic_SingleObject_NullObject_Test()
        {
            // Arrange
            var relatedEntity = new RelatedEntity
            {
                Id = "1",
                Object = null
            };

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.IsNull(result.Object);
        }

        [TestMethod]
        public void Convert_Generic_SingleObject_StringId_Test()
        {
            // Arrange
            var entity = new EntityWithDisplayCondition { Id = "ABC123", TypeId = 1 };
            var json = JsonConvert.SerializeObject(entity);
            var relatedEntity = new RelatedEntity
            {
                Id = "ABC123",
                Object = new JRaw(json)
            };

            // Act
            var result = _converter.Convert<EntityWithDisplayCondition, string>(relatedEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ABC123", result.Id);
            Assert.IsNotNull(result.Object);
            Assert.AreEqual("ABC123", result.Object.Id);
        }

        #endregion

        #region Convert(OdataObject, Type, Type) - Non-Generic Single Object

        [TestMethod]
        public void Convert_NonGeneric_SingleObject_WithAllProperties_Test()
        {
            // Arrange
            var user = new User { Id = 1, Name = "User1", UserTypeId = 5 };
            var json = JsonConvert.SerializeObject(user);
            var relatedEntity = new RelatedEntity
            {
                Id = "1",
                IdProperty = "Id",
                Uri = new Uri("/UserService.svc(1)", UriKind.Relative),
                Object = new JRaw(json),
                PropertyUris = new List<OdataUri> { new OdataUri { PropertyName = "Name", Uri = new Uri("/UserService.svc(1)/Name", UriKind.Relative) } }
            };

            // Act
            var result = _converter.Convert(relatedEntity, typeof(User), typeof(int));
            var typedResult = result as OdataObject<User, int>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(typedResult);
            Assert.AreEqual(1, typedResult.Id);
            Assert.AreEqual("Id", typedResult.IdProperty);
            Assert.AreEqual("/UserService.svc(1)", typedResult.Uri.ToString());
            Assert.IsNotNull(typedResult.Object);
            Assert.AreEqual(1, typedResult.Object.Id);
            Assert.AreEqual("User1", typedResult.Object.Name);
            Assert.AreEqual(5, typedResult.Object.UserTypeId);
        }

        [TestMethod]
        public void Convert_NonGeneric_SingleObject_NullInput_ReturnsNull_Test()
        {
            // Arrange
            RelatedEntity relatedEntity = null;

            // Act
            var result = _converter.Convert(relatedEntity, typeof(User), typeof(int));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_NonGeneric_SingleObject_NullEntityType_Throws_Test()
        {
            // Arrange
            var relatedEntity = new RelatedEntity { Id = "1" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _converter.Convert(relatedEntity, null, typeof(int)));
        }

        [TestMethod]
        public void Convert_NonGeneric_SingleObject_NullIdType_Throws_Test()
        {
            // Arrange
            var relatedEntity = new RelatedEntity { Id = "1" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _converter.Convert(relatedEntity, typeof(User), null));
        }

        #endregion

        #region Convert<TEntity, TId>(IEnumerable<OdataObject>) - Generic Collection

        [TestMethod]
        public void Convert_Generic_Collection_WithMultipleObjects_Test()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1" };
            var user2 = new User { Id = 2, Name = "User2" };
            var json1 = JsonConvert.SerializeObject(user1);
            var json2 = JsonConvert.SerializeObject(user2);

            var relatedEntity = new List<RelatedEntity>
            {
                new OdataObject { Id = "1", Object = new JRaw(json1) },
                new OdataObject { Id = "2", Object = new JRaw(json2) }
            };

            // Act
            var result = _converter.Convert<User, int>(relatedEntity);
            var resultList = result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("User1", resultList[0].Object.Name);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual("User2", resultList[1].Object.Name);
        }

        [TestMethod]
        public void Convert_Generic_Collection_NullInput_ReturnsNull_Test()
        {
            // Arrange
            IEnumerable<RelatedEntity> relatedEntities = null;

            // Act
            var result = _converter.Convert<User, int>(relatedEntities);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_Generic_Collection_EmptyCollection_Test()
        {
            // Arrange
            var relatedEntities = new List<RelatedEntity>();

            // Act
            var result = _converter.Convert<User, int>(relatedEntities);
            var resultList = result.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, resultList.Count);
        }

        #endregion

        #region Convert(IEnumerable<OdataObject>, Type, Type) - Non-Generic Collection

        [TestMethod]
        public void Convert_NonGeneric_Collection_WithMultipleObjects_Test()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "User1" };
            var user2 = new User { Id = 2, Name = "User2" };
            var json1 = JsonConvert.SerializeObject(user1);
            var json2 = JsonConvert.SerializeObject(user2);

            var relatedEntities = new List<RelatedEntity>
            {
                new RelatedEntity { Id = "1", Object = new JRaw(json1) },
                new RelatedEntity { Id = "2", Object = new JRaw(json2) }
            };

            // Act
            var result = _converter.Convert(relatedEntities, typeof(User), typeof(int));
            var typedResult = result as IEnumerable<OdataObject<User, int>>;
            var resultList = typedResult?.ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(typedResult);
            Assert.AreEqual(2, resultList.Count);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("User1", resultList[0].Object.Name);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual("User2", resultList[1].Object.Name);
        }

        [TestMethod]
        public void Convert_NonGeneric_Collection_NullInput_ReturnsNull_Test()
        {
            // Arrange
            IEnumerable<RelatedEntity> relatedEntities = null;

            // Act
            var result = _converter.Convert(relatedEntities, typeof(User), typeof(int));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Convert_NonGeneric_Collection_NullEntityType_Throws_Test()
        {
            // Arrange
            var odataObjects = new List<RelatedEntity> { new OdataObject { Id = "1" } };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _converter.Convert(odataObjects, null, typeof(int)));
        }

        [TestMethod]
        public void Convert_NonGeneric_Collection_NullIdType_Throws_Test()
        {
            // Arrange
            var relatedEntities = new List<RelatedEntity> { new RelatedEntity { Id = "1" } };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _converter.Convert(relatedEntities, typeof(User), null));
        }

        #endregion
    }
}
