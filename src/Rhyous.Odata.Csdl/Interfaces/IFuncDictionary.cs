using System;
using System.Collections.Generic;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl
{
    public interface IFuncDictionary<TKey, TFuncInput> : IConcurrentDictionary<TKey, Func<TFuncInput, IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public interface IFuncDictionary<TKey, TFuncInput1, TFuncInput2> : IConcurrentDictionary<TKey, Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>
    {
    }
}
