using System;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter
{
    public static class StringExtensions
    {
        public static string EnforceConstant<T>(this string strExpression)
        {
            var parser = new FilterExpressionParser<T>();
            Expression<Func<T, bool>> expression = null;
            bool isExpression = true;
            try { expression = parser.Parse(strExpression, false); }
            catch (Exception) { isExpression = false; }
            if (!isExpression || expression.ToString() == "f => False" )
                return strExpression;
            throw new InvalidOdataConstantException(strExpression);
        }
    }
}
