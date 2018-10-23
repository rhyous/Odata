using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlExtensions
    {
        public static CsdlEntity ToCsdl(this Type entityType)
        {
            var entity = new CsdlEntity { Keys = new List<string> { "Id" } };
            foreach (var propInfo in entityType.GetProperties())
            {
                if (propInfo.PropertyType.IsEnum)
                {
                    var csdlEnumProperty = propInfo.ToCsdlEnum();
                    if (csdlEnumProperty != null)
                        entity.Properties.Add(propInfo.Name, csdlEnumProperty);
                }
                else
                {
                    var csdlProperty = propInfo.ToCsdl();
                    if (csdlProperty != null)
                        entity.Properties.Add(propInfo.Name, csdlProperty);
                }
            }
            return entity;
        }

        private static CsdlProperty ToCsdl(this PropertyInfo property)
        {
            if (property == null)
                return null;
            var isNullable = !property.PropertyType.IsValueType;
            var propertyType = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType;

            if (propertyType.FullName != null && CsdlTypeDictionary.Instance.ContainsKey(propertyType.FullName))
            {
                return new CsdlProperty
                {
                    Type = CsdlTypeDictionary.Instance[propertyType.FullName],
                    IsCollection = property.PropertyType.IsEnumerable() || property.PropertyType.IsCollection()
                };
            }
            return null;
        }

        private static CsdlEnumProperty ToCsdlEnum(this PropertyInfo property)
        {
            if (property == null)
                return null;
            var isNullable = !property.PropertyType.IsValueType;
            var propertyType = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType;
            if (!propertyType.IsEnum)
                return null;
            return new CsdlEnumProperty
            {
                UnderlyingType = CsdlTypeDictionary.Instance[propertyType.GetEnumUnderlyingType().FullName],
                EnumOptions = propertyType.ToDictionary(),
                IsFlags = propertyType.GetCustomAttributes<FlagsAttribute>().Any()
            };
        }
    }
}