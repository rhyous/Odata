using Rhyous.Collections;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    /// <summary>
    /// A dictionary of actions based on characters found in the $filter expression string.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public class FilterExpressionParserActionDictionary<TEntity> : Dictionary<char, Action<ParserState<TEntity>>>, 
                                                                   IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>>
    {
        #region Singleton

        private static readonly Lazy<FilterExpressionParserActionDictionary<TEntity>> Lazy = new Lazy<FilterExpressionParserActionDictionary<TEntity>>(() => new FilterExpressionParserActionDictionary<TEntity>());

        /// <summary>This singleton instance</summary>
        public static FilterExpressionParserActionDictionary<TEntity> Instance { get { return Lazy.Value; } }

        /// <summary>
        /// The constructor
        /// </summary>
        internal FilterExpressionParserActionDictionary()
        {
            Add(' ', DelimiterHandler<TEntity>.Instance.Action);
            Add(',', DelimiterHandler<TEntity>.Instance.Action);

            Add('"', QuoteHandler<TEntity>.Instance.Action);
            Add('\'', QuoteHandler<TEntity>.Instance.Action);

            Add('(', new OpenParanthesisHandler<TEntity>().Action);
            Add(')', new CloseParanthesisHandler<TEntity>().Action);
        }
        #endregion

        /// <summary>
        /// By default, the action is to Append.
        /// </summary>
        public Action<ParserState<TEntity>> DefaultValue => (state) => { state.Append(); };

        /// <summary>
        /// Returns the default action if one is not found.
        /// </summary>
        public Action<ParserState<TEntity>> DefaultValueProvider(char c)
        {
            if (char.IsWhiteSpace(c))
                return DelimiterHandler<TEntity>.Instance.Action;
            return DefaultValue;
        }
    }
}