using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.Odata.Tests.Handlers
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
            Assert.AreEqual('\'', state.Group.WrapChar);
            Assert.IsTrue(state.Group.IsOpen);
            Assert.AreEqual(0, state.Builder.Length, "Builder should be empty.");
            Assert.AreEqual(8, state.CharIndex, "CharIndex should not be updated. The loop updates it and this test bypasses the loop.");
        }
    }
}
