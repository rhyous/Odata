﻿using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public class FilterExpressionBuilder<TEntity>
    {
        public FilterExpressionBuilder(string filterString, IFilterExpressionParser<TEntity> parser = null)
        {
            FilterString = filterString;
            _Parser = parser;
        }

        public string FilterString { get; private set; }

        public IFilterExpressionParser<TEntity> Parser
        {
            get { return _Parser ?? (_Parser = new FilterExpressionParser<TEntity>()); }
            set { _Parser = value; }
        } private IFilterExpressionParser<TEntity> _Parser;


        public Expression<Func<TEntity, bool>> Expression { get { return _Expression ?? (_Expression = BuildExpression(FilterString)); } }
        private Expression<Func<TEntity, bool>> _Expression;

        public Expression<Func<TEntity, bool>> BuildExpression(string filterExpression)
        {
            return Parser.Parse(filterExpression);
        }
    }
}
