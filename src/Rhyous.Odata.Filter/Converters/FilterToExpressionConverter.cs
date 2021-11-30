using Rhyous.Collections;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    internal class FilterToExpressionConverter : IFilterToExpressionConverter
    {
        #region Singleton

        private static readonly Lazy<FilterToExpressionConverter> Lazy = new Lazy<FilterToExpressionConverter>(() => new FilterToExpressionConverter());

        /// <summary>This singleton instance</summary>
        public static IFilterToExpressionConverter Instance { get { return Lazy.Value; } }

        internal FilterToExpressionConverter() { }
        #endregion

        public Expression<Func<TEntity, bool>> Convert<TEntity>(Filter<TEntity> filter)
        {
            if (filter == null || !filter.IsComplete)
                return null;

            // Handles most operators for string and int (or similar) comparisons
            if (CommonOperatorExpressionMethods.Instance.TryGetValue(filter.Method, out Func<Expression, Expression, Expression> func))
                return CommonOperatorsExpressionBuilder.Instance.Build(filter, func);

            // Handles AND an OR operators
            if (Enum.TryParse(filter.Method, true, out Conjunction conj))
                return ConjunctionExpressionBuilder.Instance.Build(filter, conj);

            // Handles the IN operator, used to see if a property value is in an Array
            if (filter.Method.Equals("IN", StringComparison.OrdinalIgnoreCase))
                return InArrayExpressionBuilder.Instance.Build(filter);

            // Handles primitives or Guid by treating them as string
            var possiblePropName = filter.Left.ToString(); 
            var propType = typeof(TEntity).GetPropertyInfo(possiblePropName)?.PropertyType;
            if (propType.IsPrimitive || propType == typeof(Guid))
                return PrimitiveAsStringExpressionBuilder.Instance.Build(filter);

            // Handles anything else
            return DefaultExpressionBuilder.Instance.Build(filter);
        }
    }
}
