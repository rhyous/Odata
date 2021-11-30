using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// A paranthesis could exist for two reasons:
    /// 1. A method, such as: Containst(Name, 'ed')
    /// 2. Grouping, to override precidence, such as: (Name eq 'A' or Name eq 'B') and LastName eq 'C'
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    public class CloseParanthesisHandler<TEntity> : IFilterCharacterHandler<TEntity>
    {
        #region Singleton

        private static readonly Lazy<CloseParanthesisHandler<TEntity>> Lazy = new Lazy<CloseParanthesisHandler<TEntity>>(() => new CloseParanthesisHandler<TEntity>());

        /// <summary>This singleton instance</summary>
        public static CloseParanthesisHandler<TEntity> Instance { get { return Lazy.Value; } }

        internal CloseParanthesisHandler() { }

        #endregion  
        /// <summary>
        /// The Action method that will handle the a open paranethesis character.
        /// </summary>
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            if (state.AppendIfInQuoteGroup())
                return;
            state.ParenthesisGroup.Close(state.Char);
        }        
    }
}
