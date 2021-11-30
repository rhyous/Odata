using Rhyous.StringLibrary;
using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// Handles the delimiter character.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class DelimiterHandler<TEntity> : IFilterCharacterHandler<TEntity>
    {
        #region Singleton

        private static readonly Lazy<DelimiterHandler<TEntity>> Lazy = new Lazy<DelimiterHandler<TEntity>>(() => new DelimiterHandler<TEntity>());

        /// <summary>This singleton instance</summary>
        public static DelimiterHandler<TEntity> Instance { get { return Lazy.Value; } }

        internal DelimiterHandler() { }

        #endregion

        /// <summary>
        /// The Action method that will handle the a delimiter character, which is white space or comma.
        /// </summary>
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.AppendIfInQuoteGroup() || (state.AppendIfMethodIsInArray()))
                return;
            if (state.Builder.Length == 0) // Handle extra spaces
                return;
            var str = state.Builder.ToString();
            if (str.ToEnum<Operator>(true, false) != null)
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
            Conjunction? conj = str.ToEnum<Conjunction>(true, false);
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
