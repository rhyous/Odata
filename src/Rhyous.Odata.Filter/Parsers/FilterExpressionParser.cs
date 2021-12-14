using LinqKit;
using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>A class to perform $filter expression parsing</summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public class FilterExpressionParser<TEntity> : IFilterExpressionParser<TEntity>
    {
        #region Singleton

        private static readonly Lazy<FilterExpressionParser<TEntity>> Lazy = new Lazy<FilterExpressionParser<TEntity>>(()
            => new FilterExpressionParser<TEntity>(FilterExpressionParserActionDictionary<TEntity>.Instance));

        /// <summary>This singleton instance</summary>
        public static FilterExpressionParser<TEntity> Instance { get { return Lazy.Value; } }


        #endregion

        private readonly IFilterExpressionParserActionDictionary<TEntity> _ActionDictionary;

        /// <summary>The constructor</summary>
        /// <param name="actionDictionary">A dictioary of parser f ilter expression arser Actions.</param>
        public FilterExpressionParser(IFilterExpressionParserActionDictionary<TEntity> actionDictionary)
        {
            _ActionDictionary = actionDictionary;
        }

        /// <inheritdoc />
        public async Task<Filter<TEntity>> ParseAsFilterAsync(string filterExpression, bool unquote = true, ICustomFilterConvertersRunner<TEntity> customFilterConverterRunner = null)
        {
            var trimmedfilterExpression = filterExpression.Trim();
            if (trimmedfilterExpression.Length > 2 && unquote)
                trimmedfilterExpression = trimmedfilterExpression.Unquote(1).Trim();
            var state = new ParserState<TEntity>(trimmedfilterExpression);
            for (state.CharIndex = 0; state.CharIndex < state.FilterString.Length; state.CharIndex++)
            {
                _ActionDictionary.GetValueOrDefault(state.Char).Invoke(state);
            }
            state.LastApply(); // Final apply required to get the last value.
            var rootFilter = state.CurrentFilter;
            while (rootFilter.Parent != null)
                rootFilter = rootFilter.Parent;
            if (customFilterConverterRunner != null)
                rootFilter = await customFilterConverterRunner.ConvertAsync(rootFilter);
            return rootFilter;
        }

        /// <inheritdoc />
        public async Task<Expression<Func<TEntity, bool>>> ParseAsync(string filterExpression, bool unquote = true, ICustomFilterConvertersRunner<TEntity> customFilterConverterRunner = null)
        {
            var rootFilter = await ParseAsFilterAsync(filterExpression, unquote, customFilterConverterRunner);
            var starter = PredicateBuilder.New<TEntity>();
            starter.Start(rootFilter);
            return starter;
        }

        /// <inheritdoc />
        public async Task<Expression<Func<TEntity, bool>>> ParseAsync(NameValueCollection parameters, bool unquote, ICustomFilterConvertersRunner<TEntity> customFilterConverterRunner = null)
        {
            if (parameters == null || parameters.Count == 0)
                return null;
            var filterString = parameters.Get("$filter", string.Empty);
            if (string.IsNullOrWhiteSpace(filterString))
                return null;
            return await ParseAsync(filterString, unquote, customFilterConverterRunner);
        }
    }
}