using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// A paranthesis could exist for two reasons:
    /// 1. A method, such as: Containst(Name, 'ed')
    /// 2. Grouping, to override precidence, such as: (Name eq 'A' or Name eq 'B') and LastName eq 'C'
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OpenParanthesisHandler<TEntity> : IHandler<ParserState<TEntity>>
    {
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.AppendIfInQuoteGroup())
                return;
            if (state.Builder.Length == 0)
            {
                throw new NotImplementedException("Grouping expressions is not yet implemented.");
            }
            state.SetMethodIfEmpty();
        }
    }
}
