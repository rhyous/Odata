using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface IPropertyAttributeDictionary : IFuncDictionary<Type, MemberInfo>
    {
        IEnumerable<KeyValuePair<string, object>> GetRelatedEntityProperties(MemberInfo mi);
    }
}