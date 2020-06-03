using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface IFuncDictionary<TKey, TFuncInput> : IDictionary<TKey, Func<TFuncInput, IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public interface IFuncDictionary<TKey, TFuncInput1, TFuncInput2> : IDictionary<TKey, Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>
    {
    }
}
