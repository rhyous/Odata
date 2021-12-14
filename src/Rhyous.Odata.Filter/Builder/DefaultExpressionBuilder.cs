using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata.Filter
{
    internal class DefaultExpressionBuilder
    {
        #region Singleton

        private static readonly Lazy<DefaultExpressionBuilder> Lazy = new Lazy<DefaultExpressionBuilder>(() => new DefaultExpressionBuilder());

        /// <summary>This singleton instance</summary>
        public static DefaultExpressionBuilder Instance { get { return Lazy.Value; } }

        internal DefaultExpressionBuilder() { }

        #endregion
        
        internal BindingFlags MethodFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public Expression<Func<TEntity, bool>> Build<TEntity>(Filter<TEntity> filter)
        {
            var possiblePropName = filter.Left.ToString();
            var propType = typeof(TEntity).GetPropertyInfo(possiblePropName)?.PropertyType;
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            Expression left = filter.Left.IsSimpleString ? Expression.Property(lambdaParameter, possiblePropName) as Expression : filter.Left;
            Expression right = propType != null && filter.Right.IsSimpleString ? Expression.Constant(filter.Right.ToString().ToType(propType)) as Expression : filter.Right;
            var methodInfo = propType.GetMethod(filter.Method, MethodFlags, null, new[] { propType }, null);
            if (methodInfo == null)
                throw new InvalidTypeMethodException(propType, filter.Method);
            var methodExpression = Expression.Call(left, methodInfo, right);
            return Expression.Lambda<Func<TEntity, bool>>(filter.Not ? Expression.Not(methodExpression) as Expression : methodExpression, lambdaParameter);
        }
    }
}