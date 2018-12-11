using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlExtensions
    {
        internal const string DefaultSchemaOrAlias = "self";

        public static CsdlEntity ToCsdl(this Type entityType, params Func<Type, IEnumerable<KeyValuePair<string, object>>>[] customPropertyBuilders)
        {
            if (entityType == null)
                return null;
            var entity = new CsdlEntity { Keys = new List<string> { "Id" } };
            if (customPropertyBuilders != null)
            {
                entity.Properties.AddCustomProperties(entityType, customPropertyBuilders);
            }
            foreach (var propInfo in entityType.GetProperties().OrderBy(p => p.Name))
            {
                entity.Properties.AddFromPropertyInfo(propInfo);
                entity.Properties.AddFromAttributes(propInfo, PropertyAttributeDictionary.Instance);
            }
            entity.Properties.AddFromAttributes(entityType, EntityAttributeDictionary.Instance);
            return entity;
        }

        public static CsdlProperty ToCsdl(this PropertyInfo propInfo)
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
            prop.CustomData.AddFromAttributes(propInfo, PropertyDataAttributeDictionary.Instance);
            return prop;
        }

        public static CsdlEnumProperty ToCsdlEnum(this PropertyInfo propInfo)
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
            prop.CustomData.AddFromAttributes(propInfo, PropertyDataAttributeDictionary.Instance);
            return prop;
        }

        public static bool IsNullable(this Type type, PropertyInfo pi)
        {
            if (type == typeof(string))
                return pi?.GetCustomAttribute<RequiredAttribute>() == null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            return !type.IsValueType;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = DefaultSchemaOrAlias, params KeyValuePair<string,object>[] additionalCustomProperties)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                Nullable = relatedEntityAttribute.Nullable,
                IsCollection = false, // RelatedEntityAttribute on a property is never a collection.
                ReferentialConstraint = new Dictionary<string, string> { { relatedEntityAttribute.Property, relatedEntityAttribute.ForeignKeyProperty } }
            };
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.Add("@Odata.Filter", relatedEntityAttribute.Filter);
            navProp.CustomData.AddRangeIfNewAndNotNull(additionalCustomProperties);
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = DefaultSchemaOrAlias, params KeyValuePair<string, object>[] additionalCustomProperties)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            navProp.CustomData.Add("@EAF.RelatedEntity.Type", "Foreign");
            navProp.CustomData.AddRangeIfNewAndNotNull(additionalCustomProperties);
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = DefaultSchemaOrAlias, params KeyValuePair<string, object>[] additionalCustomProperties)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            navProp.CustomData.Add("@EAF.RelatedEntity.Type", "Mapping");
            navProp.CustomData.AddRangeIfNewAndNotNull(additionalCustomProperties);
            return navProp;
        }
    }
}