using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface IEntityAttributeDictionary : IFuncDictionary<Type, MemberInfo>
    {
        IEnumerable<KeyValuePair<string, object>> GetDisplayProperty(MemberInfo mi);
        IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi);
        IEnumerable<KeyValuePair<string, object>> GetRelatedEntityForeignProperties(MemberInfo mi);
        IEnumerable<KeyValuePair<string, object>> GetRelatedEntityMappingProperties(MemberInfo mi);
        IEnumerable<KeyValuePair<string, object>> GetRequiredProperty(MemberInfo mi);
    }
}