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

        public PropertyBuilder(ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                               ICustomPropertyDataAppender customPropertDataAppender,
                               ICsdlTypeDictionary csdlTypeDictionary)
        {
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            _CustomPropertDataAppender = customPropertDataAppender;
            _CsdlTypeDictionary = csdlTypeDictionary;
        }

        public CsdlProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType;
            Type nullableType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? propertyType.GetGenericArguments()[0] : null;
            var propTypeName = nullableType == null ? propertyType.FullName : nullableType.FullName;
            var csdlAttribute = propInfo.GetCustomAttribute<CsdlPropertyAttribute>();
            var csdlType = csdlAttribute?.CsdlType;
            if (string.IsNullOrWhiteSpace(csdlType) && !_CsdlTypeDictionary.TryGetValue(propTypeName, out csdlType))
                return null;
            var isNullable = propInfo.IsNullable(csdlAttribute);
            var prop = new CsdlProperty
            {
                Type = csdlType,
                IsCollection = propertyType != typeof(string) && (propertyType.IsEnumerable() || propertyType.IsCollection()),
                Nullable = isNullable,
                DefaultValue = csdlAttribute?.DefaultValue,
                MaxLength = csdlAttribute?.MaxLength ?? 0,
                Precision = csdlAttribute?.Precision ?? 0,
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
                    prop.CustomData.Remove(CsdlConstants.UIRequired);
            }
        }
    }
}
