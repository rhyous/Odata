using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    internal class CommonOperatorsExpressionBuilder
    {
        #region Singleton

        private static readonly Lazy<CommonOperatorsExpressionBuilder> Lazy = new Lazy<CommonOperatorsExpressionBuilder>(() => new CommonOperatorsExpressionBuilder());

        /// <summary>This singleton instance</summary>
        public static CommonOperatorsExpressionBuilder Instance { get { return Lazy.Value; } }

        internal CommonOperatorsExpressionBuilder() { }

        #endregion
        public Expression<Func<TEntity, bool>> Build<TEntity>(Filter<TEntity> filter, Func<Expression, Expression, Expression> func)
        {
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            var possiblePropName = filter.Left.ToString();
            var property = Expression.Property(lambdaParameter, possiblePropName);
            Expression left = filter.Left.IsSimpleString ? Expression.Property(lambdaParameter, possiblePropName) as Expression : filter.Left;
            Expression right = filter.Right.IsSimpleString ? Expression.Constant(filter.Right.ToString().ToType(property.Type)) as Expression : filter.Right; ;
            var methodExpression = func.Invoke(left, right);
            return (filter.Not)
                 ? Expression.Lambda<Func<TEntity, bool>>(Expression.Not(methodExpression), lambdaParameter)
                 : Expression.Lambda<Func<TEntity, bool>>(methodExpression, lambdaParameter);
        }
    }
}
