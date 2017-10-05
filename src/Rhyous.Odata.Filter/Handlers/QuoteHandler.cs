using System;

namespace Rhyous.Odata
{
    public class QuoteHandler<TEntity> : IHandler<ParserState<TEntity>>
    {
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.QuoteGroup.IsOpen)
            {
                ((state.QuoteGroup.WrapChar == state.Char) ? state.QuoteGroup.Close : (Action)state.Append).Invoke();
                return;
            }
            state.QuoteGroup.Open(state.Char);
        }
    }
}
