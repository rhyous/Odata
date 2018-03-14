using LinqKit;
using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public class FilterExpressionParser<TEntity> : IFilterExpressionParser<TEntity>
    {
        public Expression<Func<TEntity, bool>> Parse(string filterExpression)
        {
            var state = new ParserState<TEntity>(filterExpression.Trim());
            for (state.CharIndex = 0; state.CharIndex < state.FilterString.Length; state.CharIndex++)
            {
                ActionDictionary.GetValueOrDefault(state.Char).Invoke(state);
            }            
            state.LastApply(); // Final apply required to get the last value.
            ExpressionStarter<TEntity> starter = PredicateBuilder.New<TEntity>();
            var rootFilter = state.CurrentFilter;
            while (rootFilter.Parent != null)
                rootFilter = rootFilter.Parent;
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
