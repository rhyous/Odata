using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    internal static class DictionaryExtensions
    {
        public static void AddIfNotNull(this Dictionary<string, object> dictionary, string key, object value)
        {
            if (!string.IsNullOrWhiteSpace(key) || value != null)
                dictionary.Add(key, value);                
        }
    }
}