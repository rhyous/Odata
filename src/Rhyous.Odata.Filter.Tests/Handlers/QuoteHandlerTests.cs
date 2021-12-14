using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Filter.Tests.Handlers
{
    [TestClass]
    public class QuoteHandlerTests
    {
        [TestMethod]
        public void QuoteHandlerOpenTest()
        {
            // Arrange
            string prop = "Name";
            string method = "eq";
            string value = "Jared Barneck";
            var handler = new QuoteHandler<Entity1>();
            var state = new ParserState<Entity1>($"{prop} {method} '{value}'");
            state.CurrentFilter.Left = prop;
            state.CurrentFilter.Method = method;
            state.CharIndex = 8;

            // Act
            handler.Action(state);

            // Assert
            Assert.AreEqual('\'', state.QuoteGroup.WrapChar);
            Assert.IsTrue(state.QuoteGroup.IsOpen);
            Assert.AreEqual(0, state.Builder.Length, "Builder should be empty.");
            Assert.AreEqual(8, state.CharIndex, "CharIndex should not be updated. The loop updates it and this test bypasses the loop.");
        }

        [TestMethod]
        public void QuoteHandlerApostropheTest()
        {
            // Arrange
            string prop = "Name";
            string method = "eq";
            string value = "Charlse O'Brien";
            var handler = new QuoteHandler<Entity1>();
            var state = new ParserState<Entity1>($"{prop} {method} '{value}'");
            state.CurrentFilter.Left = prop;
            state.CurrentFilter.Method = method;
            state.CharIndex = 8;

            // Act
            handler.Action(state);

            // Assert
            Assert.AreEqual('\'', state.QuoteGroup.WrapChar);
            Assert.IsTrue(state.QuoteGroup.IsOpen);
            Assert.AreEqual(0, state.Builder.Length, "Builder should be empty.");
            Assert.AreEqual(8, state.CharIndex, "CharIndex should not be updated. The loop updates it and this test bypasses the loop.");
        }


        [TestMethod]
        public void QuoteHandler_SingleQuoted_StartsWithSingleQuotes_Test()
        {
            // Arrange

            string prop = "Name";
            string method = "eq";
            string value =  "''";
            var handler = new QuoteHandler<Entity1>();
            var state = new ParserState<Entity1>($"{prop} {method} '{value}'");
            state.CurrentFilter.Left = prop;
            state.CurrentFilter.Method = method;
            state.CharIndex = 8;

            // Act
            handler.Action(state);
            state.CharIndex++;
            handler.Action(state);
            state.CharIndex++;
            handler.Action(state);

            // Assert
            Assert.AreEqual('\'', state.QuoteGroup.WrapChar);
            Assert.IsTrue(state.QuoteGroup.IsOpen);
            Assert.AreEqual(1, state.Builder.Length, "Builder should be empty.");
        }
    }
}
