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
            if (string.IsNullOrWhiteSpace(key) || value == null || dictionary.TryGetValue(key, out _))
                return;
            dictionary.Add(key, value);
        }

        public static void AddIfNewAndNotNull(this IDictionary<string, object> dictionary, string key, Func<object> func)
        {
            if (string.IsNullOrWhiteSpace(key) || func == null || dictionary.TryGetValue(key, out _))
                return;
            dictionary.AddIfNewAndNotNull(key, func.Invoke());
        }

        /// <summary>
        /// Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        internal static void AddFromAttributes(this IDictionary<string, object> propertyDictionary, MemberInfo mi, IDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> attributeActionDictionary)
        {
            if (propertyDictionary == null || mi == null || attributeActionDictionary == null)
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
            if (propInfo == null)
                return;
            if (propInfo.PropertyType.IsEnum)
                dictionary.AddIfNewAndNotNull(propInfo.Name, propInfo.ToCsdlEnum());
            else
                dictionary.AddIfNewAndNotNull(propInfo.Name, propInfo.ToCsdl());
        }
        
        internal static void AddCustomProperties(this IDictionary<string, object> dictionary, Type entityType, params Func<Type, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders)
        {
            if (entityType == null || dictionary == null || customPropertyBuilders == null || !customPropertyBuilders.Any())
                return;
            foreach (var builder in customPropertyBuilders)
            {
                if (builder == null)
                    continue;
                var list = builder.Invoke(entityType);
                foreach (var kvp in list)
                {
                    dictionary.AddIfNewAndNotNull(kvp.Key, kvp.Value);
                }
            }
        }
    }
}