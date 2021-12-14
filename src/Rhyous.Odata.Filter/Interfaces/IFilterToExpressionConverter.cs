using System;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An interface for defining the signature for converting a Filter{TEntity} to an Expression{Func{TEntity, bool}}
    /// </summary>
    public interface IFilterToExpressionConverter
    {
        /// <summary>
        /// The method used to convert a Filter{TEntity} to an Expression{Func{TEntity, bool}}
        /// </summary>
        /// <typeparam name="TEntity">The entity</typeparam>
        /// <param name="filter">The Filter{TEntity} object.</param>
        /// <returns>An Expression{Func{TEntity, bool}}</returns>
        Expression<Func<TEntity, bool>> Convert<TEntity>(Filter<TEntity> filter);
    }
}