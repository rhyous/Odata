using System;

namespace Rhyous.Odata
{
    public class QuoteHandler<TEntity> : IHandler<ParserState<TEntity>>
    {
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.Char == state.PreviousChar && state.LastAppendedCharIndex < state.CharIndex - 1)
                state.Append();

            if (state.QuoteGroup.IsOpen && state.QuoteGroup.WrapChar == state.Char)
                state.QuoteGroup.Close(state.Char);
            else
                state.QuoteGroup.Open(state.Char);
        }
    }
}
