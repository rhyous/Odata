using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class EnumPropertyBuilder : IEnumPropertyBuilder
    {
        private readonly ICustomCsdlFromAttributeAppender _CustomCsdlFromAttributeAppender;
        private readonly ICustomPropertyDataAppender _CustomPropertDataAppender;
        private readonly ICsdlTypeDictionary _CsdlTypeDictionary;

        public EnumPropertyBuilder(ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                                   ICustomPropertyDataAppender customPropertDataAppender,
                                   ICsdlTypeDictionary csdlTypeDictionary)
        {
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            _CustomPropertDataAppender = customPropertDataAppender;
            _CsdlTypeDictionary = csdlTypeDictionary;
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
                UnderlyingType = _CsdlTypeDictionary[propertyType.GetEnumUnderlyingType().FullName],
                IsFlags = propertyType.GetCustomAttributes<FlagsAttribute>().Any()
            };
            foreach (var kvp in propertyType.ToDictionary())
            {
                prop.CustomData.GetOrAdd(kvp.Key, kvp.Value);
            }                    
            _CustomPropertDataAppender.Append(prop.CustomData, propInfo.DeclaringType.Name, propInfo.Name);
            _CustomCsdlFromAttributeAppender.AppendPropertiesFromEntityAttributes(prop.CustomData, propInfo);
            return prop;
        }
    }
}
