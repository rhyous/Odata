using Rhyous.StringLibrary;
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
            try
            {
                expression = parser.Parse(strExpression, false);
                if (expression.ToString() == "f => False")
                {
                    foreach (var innerQuote in new[] { '\'', '"'})
                    {
                        try 
                        {
                            expression = parser.Parse(strExpression.Quote(innerQuote), false);
                            if (expression.ToString() != "f => False")
                                break;
                        }
                        catch (Exception) { isExpression = false; }
                    }
                }
            }
            catch (Exception) { isExpression = false; }
            if (!isExpression || expression.ToString() == "f => False")
            {
                return strExpression;
            }
            throw new InvalidOdataConstantException(strExpression);
        }
    }
}
