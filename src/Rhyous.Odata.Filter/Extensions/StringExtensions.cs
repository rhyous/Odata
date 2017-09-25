using System;
using System.Linq;

namespace Rhyous.Odata
{
    public static class StringExtensions
    {
        public static T? ToEnum<T>(this string str, bool allowNumeric = true)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("", "str");
            T ret;
            var isNumeric = str.All(c => Char.IsDigit(c));
            if ((allowNumeric || !isNumeric) && Enum.TryParse(str, true, out ret))
                return ret;
            return null;
        }
    }
}
