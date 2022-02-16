using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl
{
    public class CustomPropertyAppender : ICustomPropertyAppender 
    {
        private readonly ICustomPropertyFuncs _CustomPropertyFuncs;

        public CustomPropertyAppender(ICustomPropertyFuncs customPropertyFuncs)
        {
            _CustomPropertyFuncs = customPropertyFuncs;
        }

        public void Append(IConcurrentDictionary<string, object> dictionary, string entity)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || _CustomPropertyFuncs == null
             || !_CustomPropertyFuncs.Any())
                return;
            foreach (var func in _CustomPropertyFuncs)
            {
                var kvps = func(entity);
                if (kvps != null && kvps.Any())
                {
                    foreach (var kvp in kvps)
                    {
                        dictionary.TryAdd(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        public void Append<T>(IConcurrentDictionary<string, object> dictionary, T inT, params Func<T, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders)
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
                    dictionary.TryAdd(kvp.Key, kvp.Value);
                }
            }
        }
    }
}