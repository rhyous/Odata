using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// A singleton dictionary of common odata $filter operators
    /// </summary>
    public class CommonOperatorExpressionMethods : ConcurrentDictionary<string, Func<Expression, Expression, Expression>>
    {
        private static readonly Lazy<CommonOperatorExpressionMethods> Lazy = new Lazy<CommonOperatorExpressionMethods>(() => new CommonOperatorExpressionMethods());

        /// <summary>This singleton instance</summary>
        public static CommonOperatorExpressionMethods Instance { get { return Lazy.Value; } }

        internal CommonOperatorExpressionMethods() : base(StringComparer.OrdinalIgnoreCase)
        {
            GetOrAdd("=", (a, b) => Expression.Equal(a, b));
            GetOrAdd("eq", (a, b) => Expression.Equal(a, b));
            GetOrAdd("ne", (a, b) => Expression.NotEqual(a, b));
            GetOrAdd("!=", (a, b) => Expression.NotEqual(a, b));
            GetOrAdd("gt", (a, b) => Expression.GreaterThan(a, b));
            GetOrAdd(">", (a, b) => Expression.GreaterThan(a, b));
            GetOrAdd("ge", (a, b) => Expression.GreaterThanOrEqual(a, b));
            GetOrAdd(">=", (a, b) => Expression.GreaterThanOrEqual(a, b));
            GetOrAdd("lt", (a, b) => Expression.LessThan(a, b));
            GetOrAdd("<", (a, b) => Expression.LessThan(a, b));
            GetOrAdd("le", (a, b) => Expression.LessThanOrEqual(a, b));
            GetOrAdd("<=", (a, b) => Expression.LessThanOrEqual(a, b));
        }
    }
}