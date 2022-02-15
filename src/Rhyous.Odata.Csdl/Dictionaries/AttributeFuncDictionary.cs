using Rhyous.Collections;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{

    [ExcludeFromCodeCoverage] // Generated code that wraps ConcurrentDictionary
    public abstract class AttributeFuncDictionary : ConcurrentDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>, IFuncDictionary<Type, MemberInfo>
    {
    }
}
