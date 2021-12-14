using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var parser = new FilterExpressionParser<T>(FilterExpressionParserActionDictionary<T>.Instance);
            Expression<Func<T, bool>> expression = null;
            bool isExpression = true;
            try
            {
                expression = parser.ParseAsync(value, false).Result;
                if (expression.ToString() == "f => False")
                {
                    foreach (var innerQuote in new[] { '\'', '"'})
                    {
                        try 
                        {
                            expression = parser.ParseAsync(value.Quote(innerQuote), false).Result;
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

        /// <summary>Whether the string is wrapped in quotes or not.</summary>
        /// <param name="str">The string</param>
        /// <param name="quoteCharacters">The quote characters. If none are passed, both " and ' are used.</param>
        /// <returns>True if quoted, false otherwise.</returns>        
        internal static bool IsQuoted(this string str, params char[] quoteCharacters)
        {
            if (quoteCharacters.Length == 0)
                quoteCharacters = new[] { '\'', '"' };

            // If first and last character, aren't quotes and the same quote
            var first = str[0];
            var last = str[str.Length - 1];
            if (!quoteCharacters.Contains(first) || !quoteCharacters.Contains(last) || first != last)
                return false;
            Stack<char> quoteStack = new Stack<char>();
            quoteStack.Push(str[0]);
            for (int i = 1; i < str.Length - 1; i++) // Skip checking first and last as we already did that
            {
                if (!quoteCharacters.Contains(str[i]))
                    continue;
                if (str[i] == str[i + 1])
                {
                    i++;
                    continue;
                }
                // Check if we are closing 
                if (quoteStack.Peek() == str[i])
                    return false;
            }
            return true;
        }
    }
}
