using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Used to enforce that a value is not an expression. Throws an exception if a 
        /// value expected to be a constant is an expression.
        /// </summary>
        /// <typeparam name="T">The type of the model that might have a property with the constant value.</typeparam>
        /// <param name="value">The string that should only contain a constant value.</param>
        /// <returns>The string if it is a constant, throws otherwise.</returns>
        /// <exception cref="InvalidOdataConstantException"></exception>
        public static string EnforceConstant<T>(this string value)
        {
            var parser = new FilterExpressionParser<T>();
            Expression<Func<T, bool>> expression = null;
            bool isExpression = true;
            try
            {
                expression = parser.Parse(value, false);
                if (expression.ToString() == "f => False")
                {
                    foreach (var innerQuote in new[] { '\'', '"'})
                    {
                        try 
                        {
                            expression = parser.Parse(value.Quote(innerQuote), false);
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
                return value;
            }
            throw new InvalidOdataConstantException(value);
        }
    }
}
