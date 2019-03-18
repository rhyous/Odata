using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface IFuncDictionary<TKey> : IDictionary<TKey, Func<IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public interface IFuncDictionary<TKey, TFuncInput> : IDictionary<TKey, Func<TFuncInput, IEnumerable<KeyValuePair<string, object>>>>
    {
    }
}
