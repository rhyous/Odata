using System;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// A paranthesis could exist for two reasons:
    /// 1. A method, such as: Containst(Name, 'ed')
    /// 2. Grouping, to override precidence, such as: (Name eq 'A' or Name eq 'B') and LastName eq 'C'
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class OpenParanthesisHandler<TEntity> : IFilterCharacterHandler<TEntity>
    {
        #region Singleton

        private static readonly Lazy<OpenParanthesisHandler<TEntity>> Lazy = new Lazy<OpenParanthesisHandler<TEntity>>(() => new OpenParanthesisHandler<TEntity>());

        /// <summary>This singleton instance</summary>
        public static OpenParanthesisHandler<TEntity> Instance { get { return Lazy.Value; } }

        internal OpenParanthesisHandler() { }

        #endregion

        /// <summary>
        /// The Action method that will handle the a open paranethesis character.
        /// </summary>
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.AppendIfInQuoteGroup())
                return;
            var parenthesisType = state.Builder.Length == 0 ? ParenthesisType.Group : ParenthesisType.Method;
            if (parenthesisType == ParenthesisType.Method)
                state.SetMethodIfEmpty();
            state.ParenthesisGroup.Open(state.Char);
        }
    }
}