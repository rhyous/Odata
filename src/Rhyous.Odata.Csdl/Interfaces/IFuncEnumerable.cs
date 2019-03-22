using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface IFuncEnumerable<TFuncInput1, TFuncInput2> : IEnumerable<Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public class FuncList<TFuncInput1, TFuncInput2> :
          List<Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>,
          IFuncEnumerable<TFuncInput1, TFuncInput2> 
    {
    }
}
