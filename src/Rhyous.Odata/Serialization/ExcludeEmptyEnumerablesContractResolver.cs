using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Concurrent;
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
        
        internal void IgnoreEmptyLists(IList<JsonProperty> orderedProperties)
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

        internal ConcurrentDictionary<JsonProperty, PropertyInfo> Cache = new ConcurrentDictionary<JsonProperty, PropertyInfo>();

        internal bool ShouldSerialize(object o, JsonProperty prop)
        {
            try
            {
                if (o == null)
                    return false;
                if (!Cache.TryGetValue(prop, out PropertyInfo propInfo))
                {
                    propInfo = o.GetType().GetProperty(prop.UnderlyingName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    Cache[prop] = propInfo;
                }
                var value = propInfo.GetValue(o);
                bool b = ShouldSerialize(value);
                return b;
            }
            catch (Exception)
            {
                return true;
            }
        }

        internal bool ShouldSerialize(object value)
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