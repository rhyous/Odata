using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    internal static class DictionaryExtensions
    {
        public static void AddIfNewAndNotNull(this IDictionary<string, object> dictionary, string key, object value)
        {
            if (dictionary == null || string.IsNullOrWhiteSpace(key) || value == null || dictionary.TryGetValue(key, out _))
                return;
            dictionary.Add(key, value);
        }

        public static void AddIfNewAndNotNull(this IDictionary<string, object> dictionary, string key, Func<object> valueProvider)
        {
            if (dictionary == null || string.IsNullOrWhiteSpace(key) || valueProvider == null || dictionary.TryGetValue(key, out _))
                return;
            dictionary.AddIfNewAndNotNull(key, valueProvider.Invoke());
        }

        public static void AddRangeIfNewAndNotNull(this IDictionary<string, object> dictionary, params KeyValuePair<string, object>[] keyValuePairs)
        {
            if (dictionary == null || keyValuePairs == null)
                return;
            foreach (var kvp in keyValuePairs)
                dictionary.Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        internal static void AddFromAttributes(this IDictionary<string, object> propertyDictionary, MemberInfo mi, IDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> attributeActionDictionary)
        {
            if (propertyDictionary == null || propertyDictionary == null || mi == null || attributeActionDictionary == null)
                return;
            var attribs = mi.GetCustomAttributes(true);
            if (attribs == null)
                return;
            foreach (var attrib in attribs)
            {
                if (attributeActionDictionary.TryGetValue(attrib.GetType(), out Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> action))
                {
                    var propertyList = action.Invoke(mi);
                    foreach (var prop in propertyList)
                    {
                        propertyDictionary.AddIfNewAndNotNull(prop.Key, prop.Value);
                    }
                }
            }
        }
        
        internal static void AddFromPropertyInfo(this IDictionary<string, object> dictionary, PropertyInfo propInfo)
        {
            if (dictionary == null || propInfo == null)
                return;
            if (propInfo.PropertyType.IsEnum)
                dictionary.AddIfNewAndNotNull(propInfo.Name, propInfo.ToCsdlEnum());
            else
                dictionary.AddIfNewAndNotNull(propInfo.Name, propInfo.ToCsdl());
        }
        
        internal static void AddCustomProperties<T>(this IDictionary<string, object> dictionary, T inT, params Func<T, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders)
        {
            if (inT == null || dictionary == null || customPropertyBuilders == null || !customPropertyBuilders.Any())
                return;
            foreach (var builder in customPropertyBuilders)
            {
                if (builder == null)
                    continue;
                var list = builder.Invoke(inT);
                foreach (var kvp in list)
                {
                    dictionary.AddIfNewAndNotNull(kvp.Key, kvp.Value);
                }
            }
        }
    }
}