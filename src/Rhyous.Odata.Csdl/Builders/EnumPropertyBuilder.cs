using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class EnumPropertyBuilder : ICsdlBuilder<PropertyInfo, CsdlEnumProperty>
    {
        private IFuncDictionary<Type, MemberInfo> _PropertyDataAttributeDictionary;
        private readonly IFuncEnumerable<string, string> _CustomPropertyDataFuncs;

        public EnumPropertyBuilder(IFuncDictionary<Type, MemberInfo> propertyDataAttributeFuncDictionary,
                                  IFuncEnumerable<string, string> customPropertyDataFuncDictionary)
        {
            _PropertyDataAttributeDictionary = propertyDataAttributeFuncDictionary;
            _CustomPropertyDataFuncs = customPropertyDataFuncDictionary;
        }

        public CsdlEnumProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType.IsGenericType ? propInfo.PropertyType.GetGenericArguments()[0] : propInfo.PropertyType;
            if (!propertyType.IsEnum)
                return null;
            var prop = new CsdlEnumProperty
            {
                UnderlyingType = CsdlTypeDictionary.Instance[propertyType.GetEnumUnderlyingType().FullName],
                CustomData = propertyType.ToDictionary(),
                IsFlags = propertyType.GetCustomAttributes<FlagsAttribute>().Any()
            };
            prop.CustomData.AddFromCustomDictionary(propInfo.DeclaringType.Name, propInfo.Name, _CustomPropertyDataFuncs);
            prop.CustomData.AddFromAttributes(propInfo, _PropertyDataAttributeDictionary);
            return prop;
        }
    }
}
