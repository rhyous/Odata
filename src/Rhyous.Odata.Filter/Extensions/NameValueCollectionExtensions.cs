using Rhyous.Collections;
using System;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public static class NameValueCollectionExtensions
    {
        internal static Expression<Func<T, bool>> GetFilterExpression<T>(this NameValueCollection parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return null;
            var filterString = parameters.Get("$filter", string.Empty);
            if (string.IsNullOrWhiteSpace(filterString))
                return null;
            return new FilterExpressionBuilder<T>(filterString, new FilterExpressionParser<T>())?.Expression;
        }
    }
}