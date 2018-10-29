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
            foreach (var propInfo in entityType.GetProperties().OrderBy(p => p.Name)) // Ordered 
            {
                if (propInfo.PropertyType.IsEnum)
                    entity.Properties.AddIfNotNull(propInfo.Name, propInfo.ToCsdlEnum());
                else
                    entity.Properties.AddIfNotNull(propInfo.Name, propInfo.ToCsdl());

                // RelatedEntity
                var relatedEntityAttribute = propInfo.GetCustomAttribute<RelatedEntityAttribute>();
                if (relatedEntityAttribute == null)
                    continue;
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                entity.Properties.AddIfNotNull(relatedEntityName, relatedEntityAttribute.ToNavigationProperty());
            }
            // RelatedEntityForiegn
            foreach (var relatedEntityAttribute in entityType.GetCustomAttributes<RelatedEntityForeignAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                entity.Properties.AddIfNotNull(relatedEntityName, relatedEntityAttribute.ToNavigationProperty());
            }
            // RelatedEntityMapping
            foreach (var relatedEntityAttribute in entityType.GetCustomAttributes<RelatedEntityMappingAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                entity.Properties.AddIfNotNull(relatedEntityName, relatedEntityAttribute.ToNavigationProperty());
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

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                IsNullable = relatedEntityAttribute.Nullable,
                IsCollection = false, // RelatedEntityAttribute on a property is never a collection.
                ReferentialConstraint = new Dictionary<string, string> { { relatedEntityAttribute.Property, relatedEntityAttribute.ForeignKeyProperty } }                
            };
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var currentEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) ? relatedEntityAttribute.Entity : relatedEntityAttribute.EntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                IsNullable = true    // Collections can always be empty
            };
            return navProp;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = "self")
        {
            var relatedEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var currentEntity = string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) ? relatedEntityAttribute.Entity : relatedEntityAttribute.EntityAlias;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                IsNullable = true    // Collections can always be empty
            };
            return navProp;
        }
    }
}