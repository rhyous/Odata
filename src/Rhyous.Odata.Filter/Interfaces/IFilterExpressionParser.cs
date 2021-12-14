using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An interface to define $filter expression parsing
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public interface IFilterExpressionParser<TEntity>
    {
        /// <summary>
        /// A method to parse a $filter expression string and turn it into an Filter{TEntity}.
        /// </summary>
        /// <param name="filterExpression">The $filter expression to parse.</param>
        /// <param name="unquote">Whether to unquote or not</param>
        /// <param name="customFilterConverter">Optional. Default is null. An ICustomFilterConverter{TEntity} instance.</param>
        /// <returns>A Filter{TEntity}</returns>
        Task<Filter<TEntity>> ParseAsFilterAsync(string filterExpression, bool unquote = true, ICustomFilterConvertersRunner<TEntity> customFilterConverter = null);

        /// <summary>
        /// A method to parse a $filter expression string and turn it into an Expression{Func{TEntity, bool}}.
        /// </summary>
        /// <param name="filterExpression">The $filter expression to parse.</param>
        /// <param name="unquote">Optional. Default is true. Whether to unquote or not.</param>
        /// <param name="customFilterConverter">Optional. Default is null. An ICustomFilterConverter{TEntity} instance.</param>
        /// <returns>An Expression{Func{TEntity, bool}}</returns>
        /// <remarks>This redirects to ParseAsFilter, then converts the Filter{TEntity} to an Expression{Func{TEntity, bool}}.</remarks>
        Task<Expression<Func<TEntity, bool>>> ParseAsync(string filterExpression, bool unquote = true, ICustomFilterConvertersRunner<TEntity> customFilterConverter = null);

        /// <summary>
        /// A method to parse a $filter expression found in a <see cref="NameValueCollection"/> and turn it into an Filter.{TEntity}.
        /// </summary>
        /// <param name="parameters">A <see cref="NameValueCollection"/> of url parameters.</param>
        /// <param name="unquote">Whether to unquote or not</param>
        /// <param name="customFilterConverter">Optional. Default is null. An ICustomFilterConverter{TEntity} instance.</param>
        /// <remarks>An Expression{Func{TEntity, bool}} representation of the provided filter.</remarks>
        Task<Expression<Func<TEntity, bool>>> ParseAsync(NameValueCollection parameters, bool unquote = true, ICustomFilterConvertersRunner<TEntity> customFilterConverter = null);
    }
}