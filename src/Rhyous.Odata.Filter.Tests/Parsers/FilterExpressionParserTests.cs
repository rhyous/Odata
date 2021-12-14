using LinqKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Tests;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter.Tests.Parsers
{
    [TestClass]
    public class FilterExpressionParserTests
    {

        private MockRepository _MockRepository;

        private Mock<ICustomFilterConverterCollection<IUser>> _MockCustomFilterConverterCollection;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCustomFilterConverterCollection = _MockRepository.Create<ICustomFilterConverterCollection<IUser>>();
        }

        private FilterExpressionParser<TEntity> CreateParser<TEntity>()
        {
            return new FilterExpressionParser<TEntity>(FilterExpressionParserActionDictionary<TEntity>.Instance);
        }

        private CustomFilterConvertersRunner<IUser> CreateCustomFilterConvertersRunner()
        {
            return new CustomFilterConvertersRunner<IUser>(_MockCustomFilterConverterCollection.Object);
        }

        #region Parse (which also tests ParseAsFilter)
        [TestMethod]
        public async Task FilterExpressionParser_SimpleParserTest()
        {
            // Arrange
            var parser = CreateParser<IUser>();
            var expression = "Id eq 1";
            string expected = "e => (e.Id == 1)";

            // Act
            var actual = await parser.ParseAsync(expression);

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        public async Task FilterExpressionParser_SimpleParser_IN_Test()
        {
            // Arrange
            var array = new[] { 1,2,};
            var starter = PredicateBuilder.New<IUser>();
            Expression<Func<IUser, bool>> expression = starter.Start(x => array.Contains(x.Id));
            var expString = expression.ToString();
            var parser = CreateParser<IUser>();
            var testexpression = "Id IN (1,2)";
            string expected = "e => value(System.Int32[]).Contains(e.Id)";

            // Act
            var actual = await parser.ParseAsync(testexpression);
            var users = new List<User>
            {
                new User{ Id = 1},
                new User{ Id = 2},
                new User{ Id = 7},
            };
            var usersFound = users.Where(actual.Compile())?.ToList();

            // Assert
            Assert.AreEqual(expected, actual.ToString());
            Assert.AreEqual(2, usersFound.Count);
            Assert.AreEqual(1, usersFound[0].Id);
            Assert.AreEqual(2, usersFound[1].Id);
        }

        [TestMethod]
        public async Task FilterExpressionParser_SimpleParser_NOTIN_Test()
        {
            // Arrange
            var array = new[] { 1, 2, };
            var starter = PredicateBuilder.New<IUser>();
            Expression<Func<IUser, bool>> expression = starter.Start(x => array.Contains(x.Id));
            var expString = expression.ToString();
            var parser = CreateParser<IUser>();
            var testexpression = "Id NOTIN (1,2)";
            string expected = "e => Not(value(System.Int32[]).Contains(e.Id))";

            // Act
            var actual = await parser.ParseAsync(testexpression);
            var users = new List<User>
            {
                new User{ Id = 1},
                new User{ Id = 2},
                new User{ Id = 7},
            };
            var usersFound = users.Where(actual.Compile())?.ToList();

            // Assert
            Assert.AreEqual(expected, actual.ToString());
            Assert.AreEqual(1, usersFound.Count);
            Assert.AreEqual(7, usersFound[0].Id);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\NaiveQueryStrings.json")]
        public async Task FilterExpressionParser_NaiveFilterParserTests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\NaiveQueryStringsSymbolOperator.json")]
        public async Task FilterExpressionParser_NaiveFilterParser_SymbolOperator_Tests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\ComplexQueryStrings.json")]
        public async Task FilterExpressionParser_ComplexFilterParserTests(TestDataRow<string> row)
        {
            // Arrange 
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), string.Format(message, expected));
        }


        [TestMethod]
        public async Task FilterExpressionParser_ComplexFilterParser_Debug_Tests()
        {
            // Arrange
            var filterstring = "Id eq 1 and Name eq 'Jared \"\"Rhyous\"\" Barneck'";
            var expected = "e => ((e.Id == 1) AndAlso (e.Name == \"Jared \"\"Rhyous\"\" Barneck\"))";
            var message = "Expression should result in this expression: {0}. 1.";
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), string.Format(message, expected));
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\QuoteQueryStrings.json")]
        public async Task FilterExpressionParser_ComplexFilterParser_Quotes_Tests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), string.Format(message, expected));
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\GroupedQueryStrings.json")]
        public async Task FilterExpressionParser_GroupedFilterParserTests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\StringMethodQueryStrings.json")]
        public async Task FilterExpressionParser_StringMethodFilterParserTests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = row.Message;
            var parser = CreateParser<Entity1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }


        [TestMethod]
        public async Task FilterExpressionParser_TwoAnds_FilterParserTests()
        {
            // Arrange
            var filterstring = "Entity eq Organization and EntityId eq 265932 and Property eq ABC";
            var expected = "e => ((e.Entity == \"Organization\") AndAlso ((e.EntityId == \"265932\") AndAlso (e.Property == \"ABC\")))";
            var message = "";
            var parser = CreateParser<AlternateId>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        public async Task FilterExpressionParser_Enum_eq_FilterParserTests()
        {
            // Arrange
            var filterstring = "Type eq 1";
            var expected = "e => (e.Type == Cool)";
            var message = "";
            var parser = CreateParser<EntityWithEnum>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        public async Task FilterExpressionParser_Enum_Contains_FilterParserTests()
        {
            // Arrange
            var filterstring = "Contains(Type, 1)";
            var parser = CreateParser<EntityWithEnum>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidTypeMethodException>(async () =>
            {
                await parser.ParseAsync(filterstring);
            });
        }

        [TestMethod]
        public async Task FilterExpressionParser_DateTime_Equals_Tests()
        {
            // Arrange
            var dateTimeOffsetzzz = DateTimeOffset.Now.ToString("zzz");
            var filterstring = $"CreateDate eq '1/1/2019 00:00:00 {dateTimeOffsetzzz}'";
            var parser = CreateParser<TestClass>();
            var expected = $"e => (e.CreateDate == 1/1/2019 12:00:00 AM {dateTimeOffsetzzz})";

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\DateTimeQueryStrings.json")]
        public async Task FilterExpressionParser_DateTime_FilterParserTests(TestDataRow<string> row)
        {
            // Arrange
            var filterstring = row.TestValue;
            var expected = row.Expected;
            var message = string.Format(row.Message, row.Expected);
            var parser = CreateParser<TestClass1>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }

        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data\DateTimeOffsetQueryStrings.json")]
        public async Task FilterExpressionParser_DateTimeOffset_FilterParserTests(TestDataRow<string> row)
        {
            // Arrange
            var dateTimezzz = DateTimeOffset.Now.ToString("zzz");
            var filterstring = string.Format(row.TestValue, dateTimezzz);
            var expected = string.Format(row.Expected, dateTimezzz);
            var message = string.Format(row.Message, row.Expected);
            var parser = CreateParser<TestClass>();

            // Act
            var actual = await parser.ParseAsync(filterstring);

            // Assert
            Assert.AreEqual(expected, actual.ToString(), message);
        }
        #endregion

        #region Convert
        [TestMethod]
        public async Task FilterExpressionParser_Convert_Test()
        {
            // Arrange
            var parser = CreateParser<IUser>();
            var converter = CreateCustomFilterConvertersRunner();
            var cloneConverter = new StripInterfaceOrClassFilterConverter<IUser>();
            var converters = new List<IFilterConverter<IUser>> { cloneConverter };
            _MockCustomFilterConverterCollection.Setup(m => m.Converters).Returns(converters);

            var filterExpression = "IUser.Id IN (1,2,3,4,5)";

            // Act
            var actual = await parser.ParseAsync(filterExpression, true, converter);

            // Assert
            Assert.AreEqual("e => value(System.Int32[]).Contains(e.Id)", actual.ToString());
        }
        #endregion

    }
}