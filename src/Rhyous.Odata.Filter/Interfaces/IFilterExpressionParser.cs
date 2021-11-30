using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    /// <summary>
    /// An interface to define $filter expression parsing
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IFilterExpressionParser<TEntity>
    {
        /// <summary>
        /// A method to parse a $filter expression string and turn it into an Expression{Func{TEntity, bool}}.
        /// </summary>
        /// <param name="filterExpression">The $filter expression to parse.</param>
        /// <param name="unquote">Whether to unquote or not</param>
        /// <returns>An Expression{Func{TEntity, bool}}</returns>
        /// <remarks>This redirects to ParseAsFilter, then converts the Filter{TEntity} to an Expression{Func{TEntity, bool}}.</remarks>
        Expression<Func<TEntity, bool>> Parse(string filterExpression, bool unquote = true);

        /// <summary>
        /// A method to parse a $filter expression string and turn it into an Filter{TEntity}.
        /// </summary>
        /// <param name="filterExpression">The $filter expression to parse.</param>
        /// <param name="unquote">Whether to unquote or not</param>
        /// <returns>A Filter{TEntity}</returns>
        Filter<TEntity> ParseAsFilter(string filterExpression, bool unquote = true);
    }
}
