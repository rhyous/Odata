using LinqKit;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public class FilterExpressionParser<TEntity> : IFilterExpressionParser<TEntity>
    {

        public Filter<TEntity> ParseForFilter(string filterExpression, bool unquote = true)
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

        public Expression<Func<TEntity, bool>> Parse(string filterExpression, bool unquote = true)
        {
            var rootFilter = ParseForFilter(filterExpression, unquote);
            var starter = PredicateBuilder.New<TEntity>();
            starter.Start(rootFilter);
            return starter;
        }

        public IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>> ActionDictionary
        {
            get { return _ActionDictionary ?? (_ActionDictionary = new FilterExpressionParserActionDictionary<TEntity>()); }
            set { _ActionDictionary = value; }
        } private IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>> _ActionDictionary;
    }
}
