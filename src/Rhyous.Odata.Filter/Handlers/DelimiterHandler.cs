using Rhyous.StringLibrary;
using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// Handles the delimiter character.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    class DelimiterHandler<TEntity> : IHandler<ParserState<TEntity>>
    {
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.AppendIfInQuoteGroup())
                return;
            if (state.Builder.Length == 0) // Handle extra spaces
                return;
            var str = state.Builder.ToString();
            if (str.ToEnum<Operator>(false) != null)
            {
                state.SetMethodIfEmpty();
                return;
            }
            if (HandleConjunction(state, str))
            {
                state.Builder.Clear();
                return;
            }
            state.Apply();
        }

        internal static bool HandleConjunction(ParserState<TEntity> state, string str)
        {
            Conjunction? conj = str.ToEnum<Conjunction>(false);
            if (conj != null)
            {
                var container = state.CurrentFilter.Contain(conj.Value);
                state.CurrentFilter = container.Right;
                return true;
            }
            return false;
        }
    }
}
