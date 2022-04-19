using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class PropertyBuilder : IPropertyBuilder
    {
        private readonly ICustomCsdlFromAttributeAppender _CustomCsdlFromAttributeAppender;
        private readonly ICustomPropertyDataAppender _CustomPropertDataAppender;
        private readonly ICsdlTypeDictionary _CsdlTypeDictionary;
        private readonly IMinLengthAttributeDictionary _MinLengthAttributeDictionary;
        private readonly IMaxLengthAttributeDictionary _MaxLengthAttributeDictionary;

        public PropertyBuilder(ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                               ICustomPropertyDataAppender customPropertDataAppender,
                               ICsdlTypeDictionary csdlTypeDictionary,
                               IMinLengthAttributeDictionary minLengthAttributeDictionary,
                               IMaxLengthAttributeDictionary maxLengthAttributeDictionary)
        {
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            _CustomPropertDataAppender = customPropertDataAppender;
            _CsdlTypeDictionary = csdlTypeDictionary;
            _MinLengthAttributeDictionary = minLengthAttributeDictionary;
            _MaxLengthAttributeDictionary = maxLengthAttributeDictionary;
        }

        public CsdlProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType;
            Type nullableType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? propertyType.GetGenericArguments()[0] : null;
            var propTypeName = nullableType == null ? propertyType.FullName : nullableType.FullName;
            var csdlPropAttribute = propInfo.GetAttributeWithInterfaceInheritance<CsdlPropertyAttribute>();
            var csdlType = csdlPropAttribute?.CsdlType;
            if (string.IsNullOrWhiteSpace(csdlType) && !_CsdlTypeDictionary.TryGetValue(propTypeName, out csdlType))
                return null;
            var isNullable = propInfo.IsNullable(csdlPropAttribute);
            var prop = new CsdlProperty
            {
                Type = csdlType,
                IsCollection = propertyType != typeof(string) && (propertyType.IsEnumerable() || propertyType.IsCollection()),
                Nullable = isNullable,
                DefaultValue = csdlPropAttribute?.DefaultValue,
                MinLength = _MinLengthAttributeDictionary.GetMinLength(propInfo),
                MaxLength = _MaxLengthAttributeDictionary.GetMaxLength(propInfo),
                Precision = propInfo.GetAttributeWithInterfaceInheritance<CsdlDecimalPropertyAttribute>()?.Precision ?? 0,
            };
            _CustomPropertDataAppender.Append(prop.CustomData, propInfo.ReflectedType.Name, propInfo.Name);
            _CustomCsdlFromAttributeAppender.AppendPropertyDataFromAttributes(prop.CustomData, propInfo);
            RemoveIsRequiredIfItMatchesNullable(prop);
            return prop;
        }

        private static void RemoveIsRequiredIfItMatchesNullable(CsdlProperty prop)
        {
            if (prop.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequired))
            {
                if ((bool)isRequired != prop.Nullable)
                    prop.CustomData.TryRemove(CsdlConstants.UIRequired, out _);
            }
        }
    }
}
