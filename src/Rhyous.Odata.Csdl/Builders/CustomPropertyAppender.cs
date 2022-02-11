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

        public void Append(IDictionary<string, object> dictionary, string entity)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || _CustomPropertyFuncs == null
             || !_CustomPropertyFuncs.Any())
                return;
            foreach (var func in _CustomPropertyFuncs)
            {
                var items = func(entity);
                if (items != null && items.Any())
                    dictionary.AddRange(items);
            }
        }

        public void Append<T>(IDictionary<string, object> dictionary, T inT, params Func<T, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders)
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