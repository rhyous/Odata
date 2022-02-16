using Rhyous.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl
{
    public class CustomPropertyDataAppender : ICustomPropertyDataAppender
    {
        private readonly ICustomPropertyDataFuncs _CustomPropertyDataFuncs;

        public CustomPropertyDataAppender(ICustomPropertyDataFuncs customPropertyDataFuncs)
        {
            _CustomPropertyDataFuncs = customPropertyDataFuncs;
        }

        public void Append(IConcurrentDictionary<string, object> dictionary, string entity, string property)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || string.IsNullOrWhiteSpace(property)
             || _CustomPropertyDataFuncs == null
             || !_CustomPropertyDataFuncs.Any())
                return;
            foreach (var func in _CustomPropertyDataFuncs)
            {
                var kvps = func(entity, property);
                if (kvps != null && kvps.Any())
                {
                    foreach (var kvp in kvps)
                    {
                        dictionary.TryAdd(kvp.Key, kvp.Value);
                    }
                }
            }
        }
    }
}