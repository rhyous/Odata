using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface IFuncList<TFuncInput1> : IList<Func<TFuncInput1, IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public class FuncList<TFuncInput1> :
          List<Func<TFuncInput1, IEnumerable<KeyValuePair<string, object>>>>,
          IFuncList<TFuncInput1>
    {
    }

    public interface IFuncList<TFuncInput1, TFuncInput2> : IList<Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>
    {
    }

    public class FuncList<TFuncInput1, TFuncInput2> :
          List<Func<TFuncInput1, TFuncInput2, IEnumerable<KeyValuePair<string, object>>>>,
          IFuncList<TFuncInput1, TFuncInput2> 
    {
    }
}
