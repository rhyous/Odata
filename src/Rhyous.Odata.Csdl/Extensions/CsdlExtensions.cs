using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Rhyous.Collections;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlExtensions
    {
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
            var isNullable = !propInfo.PropertyType.IsValueType;
            var propertyType = propInfo.PropertyType.IsGenericType ? propInfo.PropertyType.GetGenericArguments()[0] : propInfo.PropertyType;
            if (!CsdlTypeDictionary.Instance.ContainsKey(propertyType.FullName))
                return null;
            var prop = new CsdlProperty
            {
                Type = CsdlTypeDictionary.Instance[propertyType.FullName],
                IsCollection = propInfo.PropertyType != typeof(string) && (propInfo.PropertyType.IsEnumerable() || propInfo.PropertyType.IsCollection())
            };
            prop.CustomData.AddFromAttributes(propInfo, PropertyDataAttributeDictionary.Instance);
            return prop;
        }

        public static CsdlEnumProperty ToCsdlEnum(this PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var isNullable = !propInfo.PropertyType.IsValueType;
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

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            if (relatedEntityAttribute == null)
                return null;
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                Nullable = relatedEntityAttribute.Nullable,
                IsCollection = false, // RelatedEntityAttribute on a property is never a collection.
                ReferentialConstraint = new Dictionary<string, string> { { relatedEntityAttribute.Property, relatedEntityAttribute.ForeignKeyProperty } }
            };
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            if (relatedEntityAttribute == null)
                return null;
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var currentEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) ? relatedEntityAttribute.Entity : relatedEntityAttribute.EntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            if (relatedEntityAttribute == null)
                return null;
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var currentEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) ? relatedEntityAttribute.Entity : relatedEntityAttribute.EntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            return navProp;
        }
    }
}