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

        public void Append(IDictionary<string, object> dictionary, string entity, string property)
        {
            if (dictionary == null
             || string.IsNullOrWhiteSpace(entity)
             || string.IsNullOrWhiteSpace(property)
             || _CustomPropertyDataFuncs == null
             || !_CustomPropertyDataFuncs.Any())
                return;
            foreach (var func in _CustomPropertyDataFuncs)
            {
                var items = func(entity, property);
                if (items != null && items.Any())
                    dictionary.AddRange(items);
            }
        }
    }
}