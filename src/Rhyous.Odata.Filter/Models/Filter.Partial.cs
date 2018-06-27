using LinqKit;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    public partial class Filter<TEntity>
    {
        #region ToString
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(NonFilter) ? $"{Left} {Method} {Right}" : NonFilter;
        }
        #endregion

        #region implicit casts
        public static implicit operator string(Filter<TEntity> filter)
        {
            if (filter == null || !filter.IsComplete)
                return null;
            return filter.ToString();
        }

        public static implicit operator Filter<TEntity>(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            return new Filter<TEntity> { NonFilter = str };
        }

        public static implicit operator Expression<Func<TEntity, bool>>(Filter<TEntity> filter)
        {
            if (filter == null || !filter.IsComplete)
                return null;
            var possiblePropName = filter.Left.ToString();
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression left = filter.Left.IsSimpleString ? Expression.Property(parameter, possiblePropName) as Expression : filter.Left;
            Type propType = typeof(TEntity).GetPropertyInfo(possiblePropName)?.PropertyType;
            Expression right = (propType != null && filter.Right.IsSimpleString) ? Expression.Constant(filter.Right.ToString().ToType(propType)) as Expression : filter.Right;
            Expression method = null;
            if (ExpressionMethodDictionary.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func))
            {
                var isNumeric = filter.Method.All(c => Char.IsDigit(c));
                if (!isNumeric && Enum.TryParse(filter.Method, true, out Conjunction conj))
                {
                    return GetCombinedExpression(left, right, conj);
                }
                method = func.Invoke(left, right);
            }
            else
            {
                var methodInfo = propType.GetMethod(filter.Method, MethodFlags, null, new[] { propType }, null);
                if (methodInfo == null)
                {
                    if (propType.IsPrimitive || propType == typeof(Guid))
                    {
                        var toStringMethod = propType.GetMethod("ToString", MethodFlags, null, new Type[] { }, null);
                        methodInfo = typeof(string).GetMethod(filter.Method, MethodFlags, null, new[] { typeof(string) }, null);
                        left = Expression.Call(left, toStringMethod);
                        right = (propType != null && filter.Right.IsSimpleString) ? Expression.Constant(filter.Right.ToString()) as Expression : filter.Right;
                    }
                }
                method = Expression.Call(left, methodInfo, right);
            }
            return Expression.Lambda<Func<TEntity, bool>>(filter.Not ? Expression.Not(method) : method, parameter);
        }
        internal static BindingFlags MethodFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        internal static Expression<Func<TEntity, bool>> GetCombinedExpression(Expression left, Expression right, Conjunction conj)
        {
            var starter = PredicateBuilder.New<TEntity>();
            starter.Start(left as Expression<Func<TEntity, bool>>);
            return conj == Conjunction.And ? starter.And(right as Expression<Func<TEntity, bool>>) : starter.Or(right as Expression<Func<TEntity, bool>>);
        }
        #endregion
    }
}

