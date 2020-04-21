namespace Rhyous.Odata.Filter.Parsers
{
    public static class StringExtensions
    {
        public static string EnforceConstant<T>(this string strExpression)
        {
            var parser = new FilterExpressionParser<T>();
            var expression = parser.Parse(strExpression);
            if (expression.ToString() == "f => False")
                return strExpression;
            throw new InvalidOdataConstantException(strExpression);
        }
    }
}
