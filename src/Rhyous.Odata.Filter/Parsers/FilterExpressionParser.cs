using LinqKit;
using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    /// <summary>
    /// A class to perform $filter expression parsing
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public class FilterExpressionParser<TEntity> : IFilterExpressionParser<TEntity>
    {
        #region Singleton

        private static readonly Lazy<FilterExpressionParser<TEntity>> Lazy = new Lazy<FilterExpressionParser<TEntity>>(() => new FilterExpressionParser<TEntity>());

        /// <summary>This singleton instance</summary>
        public static IFilterExpressionParser<TEntity> Instance { get { return Lazy.Value; } }

        internal FilterExpressionParser() { }

        #endregion
        /// <inheritdoc />
        public Filter<TEntity> ParseAsFilter(string filterExpression, bool unquote = true)
        {
            var trimmedfilterExpression = filterExpression.Trim();
            if (trimmedfilterExpression.Length > 2 && unquote)
                trimmedfilterExpression = trimmedfilterExpression.Unquote(1).Trim();
            var state = new ParserState<TEntity>(trimmedfilterExpression);
            for (state.CharIndex = 0; state.CharIndex < state.FilterString.Length; state.CharIndex++)
            {
                ActionDictionary.GetValueOrDefault(state.Char).Invoke(state);
            }
            state.LastApply(); // Final apply required to get the last value.
            var rootFilter = state.CurrentFilter;
            while (rootFilter.Parent != null)
                rootFilter = rootFilter.Parent;
            return rootFilter;
        }

        /// <inheritdoc />
        public Expression<Func<TEntity, bool>> Parse(string filterExpression, bool unquote = true)
        {
            var rootFilter = ParseAsFilter(filterExpression, unquote);
            var starter = PredicateBuilder.New<TEntity>();
            starter.Start(rootFilter);
            return starter;
        }

        internal IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>> ActionDictionary
        {
            get { return _ActionDictionary ?? (_ActionDictionary = FilterExpressionParserActionDictionary<TEntity>.Instance); }
            set { _ActionDictionary = value; }
        } private IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>> _ActionDictionary;
    }
}
