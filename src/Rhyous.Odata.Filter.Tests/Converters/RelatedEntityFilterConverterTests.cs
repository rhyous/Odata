using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata.Csdl;
using Rhyous.Odata.Filter;
using Rhyous.Odata.Tests;
using Rhyous.Odata.Tests.Models;
using Rhyous.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests.Converters
{
    [TestClass]
    public class RelatedEntityFilterConverterTests
    {
        private MockRepository _MockRepository;
        private Mock<IRelatedEntityFilterDataProvider> _MockRelatedEntityFilterDataProvider;

        private CsdlSchema _CsdlSchema;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRelatedEntityFilterDataProvider = _MockRepository.Create<IRelatedEntityFilterDataProvider>();

            _CsdlSchema = new CsdlSchema();
        }

        private RelatedEntityFilterConverter<TEntity> CreateRelatedEntityFilterConverter<TEntity>()
        {
            return new RelatedEntityFilterConverter<TEntity>(
                _CsdlSchema,
                _MockRelatedEntityFilterDataProvider.Object);
        }

        #region CanConvert
        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_FilterNull_False()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            Filter<A> filter = null;

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_Filter_NewEmptyFilter_False()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            Filter<A> filter = new Filter<A>();

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_Filter_IsSimpleString_False()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            Filter<A> filter = "Id";

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_Filter_HasNoPeriod_False()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            Filter<A> filter = "Id eq 10";

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_PeriodLastLeftCharacter_False()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            Filter<A> filter = "B. eq 11";

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_True()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            var bName = "My B 27";
            Filter<A> filter = new Filter<A> { Left = "B.Name", Method = "EQ", Right = bName };
            _CsdlSchema.Entities.TryAdd(typeof(A).Name, typeof(A).ToCsdl());
            _CsdlSchema.Entities.TryAdd(typeof(B).Name, typeof(B).ToCsdl());


            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void RelatedEntityFilterConverter_CanConvert_Array_True()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            var bName = "My B 27";
            Filter<A> filter = new Filter<A> { Left = "B.Name", Method = "EQ", Right = new ArrayFilter<A, string> { Array = new[] { bName } } };
            _CsdlSchema.Entities.TryAdd(typeof(A).Name, typeof(A).ToCsdl());
            _CsdlSchema.Entities.TryAdd(typeof(B).Name, typeof(B).ToCsdl());

            // Act
            var result = relatedEntityFilterConverter.CanConvert(filter);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Convert
        [TestMethod]
        public async Task RelatedEntityFilterConverter_Convert_RelatedEntity_Works()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>(); 
            string bName = "My B 27";
            Filter<A> filter = new Filter<A>
            {
                Left = new Filter<A> { NonFilter = "B.Name" },
                Method = "eq",
                Right = new Filter<A> { NonFilter = bName }
            };
            _CsdlSchema.Entities.TryAdd(typeof(A).Name, typeof(A).ToCsdl());
            _CsdlSchema.Entities.TryAdd(typeof(B).Name, typeof(B).ToCsdl());
            var expectedFilter = $"$Filter=Name eq 'My B 27'";
            var b27 = new B { Id = 27, Name = bName };
            var odataObjectB27 = new OdataObject();
            var b27Json = JsonConvert.SerializeObject(b27);
            odataObjectB27.Object = new JRaw(b27Json);
            var odataBCollection = new OdataObjectCollection { odataObjectB27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(B), expectedFilter))
                                      .ReturnsAsync(odataBCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual("BId in (27)", result.ToString());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task RelatedEntityFilterConverter_Convert_RelatedEntity_PropertyOtherThandId_Works()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<User2>();
            Filter<User2> filter = new Filter<User2> { Left = "UserType.Id", Method = "eq", Right = "3" };
            _CsdlSchema.Entities.TryAdd(typeof(User2).Name, typeof(User2).ToCsdl());
            _CsdlSchema.Entities.TryAdd(typeof(UserType).Name, typeof(UserType).ToCsdl());
            var expectedUrlParams = $"$Filter=Id eq 3";
            var userType3 = new B { Id = 27, Name = "Name 3" };
            var odataObjectUserType3 = new OdataObject();
            var userType3Json = JsonConvert.SerializeObject(userType3);
            odataObjectUserType3.Object = new JRaw(userType3Json);
            var odataBCollection = new OdataObjectCollection { odataObjectUserType3 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(UserType), expectedUrlParams))
                                      .ReturnsAsync(odataBCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual("UserTypeName in ('Name 3')", result.ToString());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task RelatedEntityFilterConverter_Convert_RelatedEntity_Works_Array()
        {
            // Arrange
            var relatedEntityFilterConverter = CreateRelatedEntityFilterConverter<A>();
            var bName = "My B 27";
            Filter<A> filter = new Filter<A> { Left = "B.Name", Method = "in", Right = new ArrayFilter<A, string> { Array = new[] { bName } } };
            _CsdlSchema.Entities.TryAdd(typeof(A).Name, typeof(A).ToCsdl());
            _CsdlSchema.Entities.TryAdd(typeof(B).Name, typeof(B).ToCsdl());
            var expectedFilter = $"$Filter=Name in ('{bName}')";
            var b27 = new B { Id = 27, Name = bName };
            var odataObjectB27 = new OdataObject();
            var b27Json = JsonConvert.SerializeObject(b27);
            odataObjectB27.Object = new JRaw(b27Json);
            var odataBCollection = new OdataObjectCollection { odataObjectB27 };
            _MockRelatedEntityFilterDataProvider.Setup(m => m.ProvideAsync(nameof(B), expectedFilter))
                                      .ReturnsAsync(odataBCollection);

            // Act
            var result = await relatedEntityFilterConverter.ConvertAsync(filter);

            // Assert
            Assert.AreEqual("BId in (27)", result.ToString());
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
