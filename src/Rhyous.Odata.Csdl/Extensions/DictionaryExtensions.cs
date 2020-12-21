using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    internal static class DictionaryExtensions
    {
        internal static void AddFromCustomDictionary(this IDictionary<string, object> dictionary, string entity, IFuncList<string> customPropertyFuncs)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || customPropertyFuncs == null
             || !customPropertyFuncs.Any())
                return;
            foreach (var func in customPropertyFuncs)
            {
                var items = func(entity);
                if (items != null && items.Any())
                    dictionary.AddRange(items);
            }
        }

        internal static void AddFromCustomDictionary(this IDictionary<string, object> dictionary, string entity, string property, IFuncList<string, string> customPropertyDataFuncs)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || string.IsNullOrWhiteSpace(property)
             || customPropertyDataFuncs == null
             || !customPropertyDataFuncs.Any())
                return;
            foreach (var func in customPropertyDataFuncs)
            {
                var items = func(entity, property);
                if (items != null && items.Any())
                    dictionary.AddRange(items);
            }
        }

        /// <summary>
        /// Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        internal static void AddFromAttributes(this IDictionary<string, object> propertyDictionary, MemberInfo mi, IDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> attributeActionDictionary)
        {
            if (propertyDictionary == null || mi == null || attributeActionDictionary == null || !attributeActionDictionary.Any())
                return;
            var attribs = mi.GetCustomAttributes(true);
            if (attribs == null)
                return;
            foreach (var attrib in attribs)
            {
                if (attributeActionDictionary.TryGetValue(attrib.GetType(), out Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> action))
                {
                    var propertyList = action(mi);
                    foreach (var prop in propertyList)
                    {
                        // Don't add an attribute if already added.
                        if (propertyDictionary.TryGetValue(prop.Key, out object _))
                            continue;
                        propertyDictionary.AddIfNewAndNotNull(prop.Key, prop.Value);
                    }
                }
            }
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