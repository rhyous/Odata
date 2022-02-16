using Rhyous.Collections;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomPropertyAppender 
    {
        void Append<T>(IConcurrentDictionary<string, object> dictionary, T inT, params Func<T, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders);
        void Append(IConcurrentDictionary<string, object> dictionary, string entity);
    }
}