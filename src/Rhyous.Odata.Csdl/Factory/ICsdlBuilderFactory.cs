using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface ICsdlBuilderFactory
    {
        // Builders
        EntityBuilder EntityBuilder { get; }
        PropertyBuilder PropertyBuilder { get; }
        EnumPropertyBuilder EnumPropertyBuilder { get; }

        // Dictionaries
        IFuncList<string> CustomPropertyFuncs { get; }
        IFuncList<string, string> CustomPropertyDataFuncs { get; }

        IFuncDictionary<Type, MemberInfo> EntityAttributeDictionary { get; }
        IFuncDictionary<Type, MemberInfo> PropertyAttributeDictionary { get; }
        IFuncDictionary<Type, MemberInfo> PropertyDataAttributeDictionary { get; }
        IDictionary<string, string> CsdlTypeDictionary { get; }
    }
}