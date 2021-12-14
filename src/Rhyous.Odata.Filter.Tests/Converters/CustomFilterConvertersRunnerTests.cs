using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests.Converters
{
    [TestClass]
    public class CustomFilterConvertersRunnerTests
    {
        private MockRepository _MockRepository;

        private Mock<ICustomFilterConverterCollection<IUser>> _MockCustomFilterConverterCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomFilterConverterCollection = _MockRepository.Create<ICustomFilterConverterCollection<IUser>>();
        }

        private CustomFilterConvertersRunner<IUser> CreateCustomFilterConvertersRunner()
        {
            return new CustomFilterConvertersRunner<IUser>(_MockCustomFilterConverterCollection.Object);
        }

        #region Convert
        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_One_Filter_Converted_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            Filter<IUser> filter = "Id";

            // Act
            var actual = await converter.ConvertAsync(filter);

            // Assert
            Assert.AreNotEqual(filter, actual);
            Assert.IsFalse(string.IsNullOrWhiteSpace(filter.NonFilter));
            Assert.AreEqual(filter.NonFilter, actual.NonFilter);
            Assert.IsTrue(actual.IsRoot);
        }

        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_One_ArrayFilter_Converted_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            Filter<IUser> filter = new ArrayFilter<IUser, int> { Array = new[] { 1, 2, 3, 4, 5 } };

            // Act
            var actual = await converter.ConvertAsync(filter);

            // Assert
            Assert.IsTrue(actual.IsArray);
            Assert.AreNotEqual(filter, actual);
            Assert.IsTrue(string.IsNullOrWhiteSpace(filter.NonFilter));
            Assert.IsTrue(actual.IsRoot);
        }

        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_3Filters_Converted_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            // Left and Right are Filter<TEntity> instances with NonFilter set.
            Filter<IUser> filter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "27" };

            // Act
            var actual = await converter.ConvertAsync(filter);

            // Assert
            Assert.AreNotEqual(filter, actual);
            Assert.IsTrue(actual.IsRoot);

            Assert.AreNotSame(filter.Left, actual.Left);
            Assert.AreNotSame(filter.Right, actual.Right);
        }

        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_7Filters_Root_OR_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            Filter<IUser> leftFilter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "27" };
            Filter<IUser> rightFilter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "28" };
            Filter<IUser> orFilter = new Filter<IUser> { Left = leftFilter, Method = "or", Right = rightFilter };

            // Act
            var actual = await converter.ConvertAsync(orFilter);

            // Assert
            Assert.AreNotEqual(orFilter, actual);
            Assert.IsTrue(actual.IsRoot);

            Assert.AreNotSame(orFilter.Left, actual.Left);
            Assert.AreNotSame(orFilter.Right, actual.Right);


            Assert.AreNotSame(orFilter.Left.Left, actual.Left.Left);
            Assert.AreNotSame(orFilter.Left.Right, actual.Left.Right);

            Assert.AreNotSame(orFilter.Right.Left, actual.Right.Left);
            Assert.AreNotSame(orFilter.Right.Right, actual.Right.Right);
        }

        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_7Filters_Root_OR_Id_To_EntityId_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var idToEntityIdConverter = new IdToEntityIdFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter, idToEntityIdConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            Filter<IUser> leftFilter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "27" };
            Filter<IUser> rightFilter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "28" };
            Filter<IUser> orFilter = new Filter<IUser> { Left = leftFilter, Method = "or", Right = rightFilter };

            // Act
            var actual = await converter.ConvertAsync(orFilter);

            // Assert
            Assert.AreNotEqual(orFilter, actual);
            Assert.IsTrue(actual.IsRoot);

            Assert.AreNotSame(orFilter.Left, actual.Left);
            Assert.AreNotSame(orFilter.Right, actual.Right);


            Assert.AreNotSame(orFilter.Left.Left, actual.Left.Left);
            Assert.AreEqual("UserId", actual.Left.Left.ToString());
            Assert.AreNotSame(orFilter.Left.Right, actual.Left.Right);

            Assert.AreNotSame(orFilter.Right.Left, actual.Right.Left);
            Assert.AreEqual("UserId", actual.Right.Left.ToString());
            Assert.AreNotSame(orFilter.Right.Right, actual.Right.Right);
        }

        [TestMethod]
        public async Task CustomFilterConvertersRunner_Convert_7Filters_Root_OR_Id_To_EntityId_WithArray_Test()
        {
            // Arrange
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new CloneFilterConverter<IUser>();
            var idToEntityIdConverter = new IdToEntityIdFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter, idToEntityIdConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            Filter<IUser> leftFilter = new Filter<IUser> { Left = "Id", Method = "eq", Right = "27" };
            var rightArrayFilter = new ArrayFilter<IUser, int> { Array = new[] { 28, 29 } };
            Filter<IUser> rightFilter = new Filter<IUser> { Left = "Id", Method = "IN", Right = rightArrayFilter };
            Filter<IUser> orFilter = new Filter<IUser> { Left = leftFilter, Method = "or", Right = rightFilter };

            // Act
            var actual = await converter.ConvertAsync(orFilter);

            // Assert
            Assert.AreNotEqual(orFilter, actual);
            Assert.IsTrue(actual.IsRoot);

            Assert.AreNotSame(orFilter.Left, actual.Left);
            Assert.AreNotSame(orFilter.Right, actual.Right);


            Assert.AreNotSame(orFilter.Left.Left, actual.Left.Left);
            Assert.AreEqual("UserId", actual.Left.Left.ToString());
            Assert.AreNotSame(orFilter.Left.Right, actual.Left.Right);

            Assert.AreNotSame(orFilter.Right.Left, actual.Right.Left);
            Assert.AreEqual("UserId", actual.Right.Left.ToString());
            Assert.AreSame(typeof(ArrayFilter<IUser, int>), orFilter.Right.Right.GetType());
            Assert.AreNotSame(orFilter.Right.Right, actual.Right.Right);
        }
        #endregion
    }
}
