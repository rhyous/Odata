using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    internal class PrimitiveAsStringExpressionBuilder
    {
        #region Singleton

        private static readonly Lazy<PrimitiveAsStringExpressionBuilder> Lazy = new Lazy<PrimitiveAsStringExpressionBuilder>(() => new PrimitiveAsStringExpressionBuilder());

        /// <summary>This singleton instance</summary>
        public static PrimitiveAsStringExpressionBuilder Instance { get { return Lazy.Value; } }

        internal PrimitiveAsStringExpressionBuilder() { }

        #endregion
        
        internal BindingFlags MethodFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public Expression<Func<TEntity, bool>> Build<TEntity>(Filter<TEntity> filter)
        {
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            var possiblePropName = filter.Left.ToString();
            var property = Expression.Property(lambdaParameter, possiblePropName);
            Expression left = filter.Left.IsSimpleString ? Expression.Property(lambdaParameter, possiblePropName) as Expression : filter.Left; ;
            var toStringMethod = property.Type.GetMethod("ToString", MethodFlags, null, new Type[] { }, null);
            var methodInfo = typeof(string).GetMethod(filter.Method, MethodFlags, null, new[] { typeof(string) }, null);
            left = Expression.Call(left, toStringMethod);
            Expression right = (property.Type != null && filter.Right.IsSimpleString) ? Expression.Constant(filter.Right.ToString()) as Expression : filter.Right;
            var call = Expression.Call(left, methodInfo, right);
            return (filter.Not)
                 ? Expression.Lambda<Func<TEntity, bool>>(Expression.Not(call), lambdaParameter)
                 : Expression.Lambda<Func<TEntity, bool>>(call, lambdaParameter);
        } 
    }
}
