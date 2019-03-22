using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class PropertyBuilder : ICsdlBuilder<PropertyInfo, CsdlProperty>
    {
        private readonly IFuncDictionary<Type, MemberInfo> _PropertyDataAttributeDictionary;
        private readonly IFuncEnumerable<string, string> _CustomPropertyDataFuncs;

        public PropertyBuilder(IFuncDictionary<Type, MemberInfo> propertyDataAttributeFuncDictionary,
                               IFuncEnumerable<string, string> customPropertyDataFuncDictionary)
        {
            _PropertyDataAttributeDictionary = propertyDataAttributeFuncDictionary;
            _CustomPropertyDataFuncs = customPropertyDataFuncDictionary;
        }

        public CsdlProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType;
            Type nullableType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? propertyType.GetGenericArguments()[0] : null;
            var propName = nullableType == null ? propertyType.FullName : nullableType.FullName;
            if (!CsdlTypeDictionary.Instance.TryGetValue(propName, out string csdlType))
                return null;
            var prop = new CsdlProperty
            {
                Type = csdlType,
                IsCollection = propertyType != typeof(string) && (propertyType.IsEnumerable() || propertyType.IsCollection()),
                Nullable = propertyType.IsNullable(propInfo)
            };
            prop.CustomData.AddFromCustomDictionary(propInfo.ReflectedType.Name, propInfo.Name, _CustomPropertyDataFuncs);
            prop.CustomData.AddFromAttributes(propInfo, _PropertyDataAttributeDictionary);
            return prop;
        }
    }
}
