using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;

namespace Rhyous.Odata.Filter.Tests
{
    [TestClass]
    public class DelimiterHandlerTests
    {

        [TestMethod]
        public void DelimiterHandlerTest()
        {
            // Arrange
            string prop = "Id";
            var handler = new DelimiterHandler<Entity1>();
            var state = new ParserState<Entity1>($"{prop} eq 1");
            state.Builder.Append(prop);
            state.CharIndex = 2;

            // Act
            handler.Action(state);

            // Assert
            Assert.AreEqual(prop, state.CurrentFilter.Left.ToString(), "Filter.Property should be set.");
            Assert.AreEqual(0, state.Builder.Length, "Builder should be cleared.");
            Assert.AreEqual(2, state.CharIndex, "CharIndex should not be updated. The loop updates it and this test bypasses the loop.");
        }
    }
}
