using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public class ExpressionMethodDictionary : ConcurrentDictionary<string, Func<Expression, Expression, Expression>>
    {
        #region Singleton

        private static readonly Lazy<ExpressionMethodDictionary> Lazy = new Lazy<ExpressionMethodDictionary>(() => new ExpressionMethodDictionary());

        public static ExpressionMethodDictionary Instance { get { return Lazy.Value; } }

        internal ExpressionMethodDictionary() : base(StringComparer.OrdinalIgnoreCase)
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
            GetOrAdd("and", (a, b) => Expression.AndAlso(a, b));
            GetOrAdd("or", (a, b) => Expression.OrElse(a, b));
        }

        #endregion
    }
}
