using LinqKit;
using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    internal class ConjunctionExpressionBuilder
    {
        #region Singleton

        private static readonly Lazy<ConjunctionExpressionBuilder> Lazy = new Lazy<ConjunctionExpressionBuilder>(() => new ConjunctionExpressionBuilder());

        /// <summary>This singleton instance</summary>
        public static ConjunctionExpressionBuilder Instance { get { return Lazy.Value; } }

        internal ConjunctionExpressionBuilder() { }

        #endregion
        public Expression<Func<TEntity, bool>> Build<TEntity>(Filter<TEntity> filter, Conjunction conj)
        {
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            var possiblePropName = filter.Left.ToString(); 
            Type propType = typeof(TEntity).GetPropertyInfo(possiblePropName)?.PropertyType;
            var right = (propType != null && filter.Right.IsSimpleString) ? Expression.Constant(filter.Right.ToString().ToType(propType)) as Expression : filter.Right;
            var combinedExpression = GetCombinedExpression<TEntity>(filter.Left, right, conj);
            return (filter.Not)
                 ? Expression.Lambda<Func<TEntity, bool>>(Expression.Not(combinedExpression), lambdaParameter)
                 : combinedExpression;
        }

        internal Expression<Func<TEntity, bool>> GetCombinedExpression<TEntity>(Expression left, Expression right, Conjunction conj)
        {
            if (left == null && right == null)
                return null;
            if (left == null)
                return right as Expression<Func<TEntity, bool>>;
            if (right == null)
                return left as Expression<Func<TEntity, bool>>;
            var starter = PredicateBuilder.New<TEntity>();

            starter.Start(left as Expression<Func<TEntity, bool>>);
            var expression = conj == Conjunction.And 
                           ? starter.And(right as Expression<Func<TEntity, bool>>) 
                           : starter.Or(right as Expression<Func<TEntity, bool>>);
            return expression;
        }
    }
}