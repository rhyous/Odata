using System;

namespace Rhyous.Odata
{
    public class QuoteHandler<TEntity> : IHandler<ParserState<TEntity>>
    {
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.Group.IsOpen)
            {
                ((state.Group.WrapChar == state.Char) ? state.Group.Close : (Action)state.Append).Invoke();
                return;
            }
            state.Group.Open(state.Char);
        }
    }
}
