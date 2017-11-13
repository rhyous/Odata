using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
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
            foreach (var prop in orderedProperties)
            {
                if (prop.PropertyType == typeof(string))
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
            PropertyInfo propInfo;
            if (!Cache.TryGetValue(prop, out propInfo))
                Cache[prop] = propInfo = o.GetType().GetProperty(prop.PropertyName);
            var value = propInfo.GetValue(o);
            return ShouldSerialize(value);
        }

        internal static bool ShouldSerialize(object value)
        {
            if (value == null)
                return false;
            var collection = value as ICollection;
            if (collection != null)
                return collection.Count > 0;

            var enumerable = value as IEnumerable;
            if (enumerable == null)
                return value != null;
            var e = enumerable.GetEnumerator();
            return e.MoveNext();
        }
    }
}