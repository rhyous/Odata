using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata
{
    public class ExcludeEmptyEnumerablesContractResolver : DefaultContractResolver
    {
        #region Singleton
        private static readonly Lazy<ExcludeEmptyEnumerablesContractResolver> Lazy = new Lazy<ExcludeEmptyEnumerablesContractResolver>(() => new ExcludeEmptyEnumerablesContractResolver());
        public static ExcludeEmptyEnumerablesContractResolver Instance { get { return Lazy.Value; } }
        protected internal ExcludeEmptyEnumerablesContractResolver() { }
        #endregion
        
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            IgnoreEmptyLists(properties);
            return properties;
        }
        
        internal static void IgnoreEmptyLists(IList<JsonProperty> orderedProperties)
        {
            var skipTypes = new[] { typeof(string), typeof(JRaw) };
            foreach (var prop in orderedProperties)
            {
                if (skipTypes.Contains(prop.PropertyType))
                    continue;
                if (typeof(ICollection).IsAssignableFrom(prop.PropertyType) || typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    if (prop.ShouldSerialize == null)
                        prop.ShouldSerialize = o => { return ShouldSerialize(o, prop); };
                }
            }
        }

        internal static Dictionary<JsonProperty, PropertyInfo> Cache = new Dictionary<JsonProperty, PropertyInfo>();

        internal static bool ShouldSerialize(object o, JsonProperty prop)
        {
            if (o == null)
                return false;
            if (!Cache.TryGetValue(prop, out PropertyInfo propInfo))
                Cache[prop] = propInfo = o.GetType().GetProperty(prop.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var value = propInfo.GetValue(o);
            return ShouldSerialize(value);
        }

        internal static bool ShouldSerialize(object value)
        {
            if (value == null)
                return false;
            if (value is ICollection collection)
                return collection.Count > 0;

            var enumerable = value as IEnumerable;
            if (enumerable == null)
                return value != null;
            var e = enumerable.GetEnumerator();
            return e.MoveNext();
        }
    }
}