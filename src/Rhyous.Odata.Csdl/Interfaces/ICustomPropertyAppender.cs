using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomPropertyAppender 
    {
        void Append<T>(IDictionary<string, object> dictionary, T inT, params Func<T, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders);
        void Append(IDictionary<string, object> dictionary, string entity);
    }
}