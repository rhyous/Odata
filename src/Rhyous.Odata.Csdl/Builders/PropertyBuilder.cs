using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class PropertyBuilder : ICsdlBuilder<PropertyInfo, CsdlProperty>
    {
        private readonly IFuncDictionary<Type, MemberInfo> _PropertyDataAttributeFuncDictionary;
        private readonly IFuncDictionary<string> _CustomPropertyDataDictionary;

        public PropertyBuilder(IFuncDictionary<Type, MemberInfo> propertyDataAttributeFuncDictionary,
                               IFuncDictionary<string> customPropertyDataDictionary)
        {
            _PropertyDataAttributeFuncDictionary = propertyDataAttributeFuncDictionary;
            _CustomPropertyDataDictionary = customPropertyDataDictionary;
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
            prop.CustomData.AddFromCustomDictionary(propInfo.DeclaringType.Name, propInfo.Name, new CustomPropertyDataDictionary());
            prop.CustomData.AddFromAttributes(propInfo, new PropertyDataAttributeDictionary());
            return prop;
        }

        public CsdlEnumProperty BuildEnumProperty(PropertyInfo propInfo)
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
            prop.CustomData.AddFromAttributes(propInfo, new PropertyDataAttributeDictionary());
            return prop;
        }
    }
}
