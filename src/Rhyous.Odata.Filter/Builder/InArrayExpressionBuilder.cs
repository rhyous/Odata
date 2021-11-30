using Rhyous.Collections;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    internal class InArrayExpressionBuilder
    {
        #region Singleton

        private static readonly Lazy<InArrayExpressionBuilder> Lazy = new Lazy<InArrayExpressionBuilder>(() => new InArrayExpressionBuilder());

        /// <summary>This singleton instance</summary>
        public static InArrayExpressionBuilder Instance { get { return Lazy.Value; } }

        internal InArrayExpressionBuilder() { }

        #endregion
        public Expression<Func<TEntity, bool>> Build<TEntity>(Filter<TEntity> filter)
        {
            var lambdaParameter = Expression.Parameter(typeof(TEntity), "e");
            var possiblePropName = filter.Left.ToString();
            var property = Expression.Property(lambdaParameter, possiblePropName);
            var array = (filter.Right).GetPropertyValue("Array") as Array;
            ConstantExpression arrayExpression = null;
            var arrayType = array.GetType().GetElementType();
            // The Array will likely be an array of string, but if the property is not a string, it should be converted
            if (arrayType != property.Type && arrayType == typeof(string))
            {
                var strArray = array as string[];
                var newArray = Array.CreateInstance(property.Type, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    var x = strArray[i].ToType(property.Type);
                    newArray.SetValue(x, i);
                }
                arrayExpression = Expression.Constant(newArray, newArray.GetType());
            }
            arrayExpression = arrayExpression ?? Expression.Constant(array, array.GetType());

            var call = Expression.Call(typeof(Enumerable), "Contains", new[] { property.Type }, arrayExpression, property);
            return (filter.Not)
                 ? Expression.Lambda<Func<TEntity, bool>>(Expression.Not(call), lambdaParameter)
                 : Expression.Lambda<Func<TEntity, bool>>(call, lambdaParameter);
        }
    }
}
