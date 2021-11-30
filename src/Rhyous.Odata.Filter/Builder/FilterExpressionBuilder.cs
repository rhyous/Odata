using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    /// <summary>
    /// A builder that creates an Expression from 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [Obsolete("Use FilterExpressionParser<TEntity> instead.")]
    public class FilterExpressionBuilder<TEntity>
    {
        /// <summary>
        /// The FilterExpressionBuilder constructor
        /// </summary>
        /// <param name="filterString">The $filter expression string.</param>
        /// <param name="parser">Optional. An IFilterExpressionParser{TEntity} instance.</param>
        public FilterExpressionBuilder(string filterString, IFilterExpressionParser<TEntity> parser = null)
        {
            FilterString = filterString;
            _Parser = parser;
        }

        /// <summary>The $filter expression string.</summary>
        public string FilterString { get; private set; }

        /// <summary>An instance of IFilterExpressionParser{TEntity}.</summary>
        public IFilterExpressionParser<TEntity> Parser
        {
            get { return _Parser ?? (_Parser = FilterExpressionParser<TEntity>.Instance); }
            set { _Parser = value; }
        } private IFilterExpressionParser<TEntity> _Parser;

        /// <summary>The built expression.</summary>
        public Expression<Func<TEntity, bool>> Expression { get { return _Expression ?? (_Expression = Parser.Parse(FilterString)); } }
        private Expression<Func<TEntity, bool>> _Expression;
    }
}