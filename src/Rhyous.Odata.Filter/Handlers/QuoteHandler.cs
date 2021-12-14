using System;
using System.Linq;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// A handler for parsing Quotes in a $Filter expression string.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity being queried.</typeparam>
    public class QuoteHandler<TEntity> : IFilterCharacterHandler<TEntity>
    {
        #region Singleton

        private static readonly Lazy<QuoteHandler<TEntity>> Lazy = new Lazy<QuoteHandler<TEntity>>(() => new QuoteHandler<TEntity>());

        /// <summary>This singleton instance</summary>
        public static QuoteHandler<TEntity> Instance { get { return Lazy.Value; } }

        internal QuoteHandler() { }

        #endregion

        /// <summary>
        /// The Action method that will handle the quote character.
        /// </summary>
        public Action<ParserState<TEntity>> Action => HandlerMethod;

        internal void HandlerMethod(ParserState<TEntity> state)
        {
            // Two quotes escape a quote
            if (state.Char == state.PreviousChar && state.LastAppendedCharIndex < state.PreviousCharIndex)
            {   // Inner quotes of a quote group of different quote characters will never reach here.
                state.Append();
                if (state.QuoteGroup.IsOpen)
                    // If a group is open, close it. Example, when escaped quotes are provided inside an unquoted string: O''Brien.
                    state.QuoteGroup.Close(state.Char);
                else
                    // If the last quote closed the group, reopen it. Example, when escaped quotes are inside a same-character-quoted string: 'O''Brien'
                    state.QuoteGroup.Open(state.Char);
                return;
            }

            if (state.QuoteGroup.IsOpen)
            {
                if (state.QuoteGroup.WrapChar == state.Char)
                    // If it is a quote inside a quote group of the same quote type, close the group.
                    state.QuoteGroup.Close(state.Char);
                else
                    // It is a different quote character, so the outer quotes are used to escape these quotes
                    // Single quotes are considered escaped if inside double quote groups. "O'Brien"
                    // Double quotes are considered escaped if inside single quote groups. '"Wow!", she said.'
                    state.Append();
                return;
            }
            // Quote group isn't open if you reach here

            if (state.IsLastChar || state.RemainingFilterString.All(c => c == ')'))
            {
                // If the last character is a quote, assume it is escaped. Such as when:
                // 1. The quote is the only character.
                // 2. A word like: Runnin'
                // 3. It is the last character before all the closing parentheses: ((Name eq Runnin) or (Name eq Runnin'))
                //    Notice that the reverse is valid syntax only if there is only one total single quote: ((Name eq Runnin') or (Name eq Runnin))
                //    If there are additional quotes, it is broken: ((Name eq Runnin') or (Name eq 'Runnin'))
                state.Append();
                return;
            }

            // Quote group is closed, it isn't the last character, and there is no other quote remaining. Such as in:
            // 1. O'Brien
            if (state.RemainingFilterString.IndexOf(state.Char) < 0)
            {
                state.Append();
                return;
            }

            state.QuoteGroup.Open(state.Char);
        }
    }
}
