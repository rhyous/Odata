using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlExtensions
    {
        public static CsdlEntity ToCsdl(this Type entityType, EntityBuilder entityBuilder = null)
        {
            entityBuilder = entityBuilder ?? new CsdlBuilderFactory().CreateEntityBuilder();
            return entityBuilder.Build(entityType);
        }

        public static CsdlProperty ToCsdl(this PropertyInfo propInfo, PropertyBuilder propertyBuilder = null)
        {
            propertyBuilder = propertyBuilder ?? new CsdlBuilderFactory().CreatePropertyBuilder();
            return propertyBuilder.Build(propInfo);
        }

        public static CsdlEnumProperty ToCsdlEnum(this PropertyInfo propInfo, EnumPropertyBuilder enumPropertyBuilder = null)
        {
            enumPropertyBuilder = enumPropertyBuilder ?? new CsdlBuilderFactory().CreateEnumPropertyBuilder();
            return enumPropertyBuilder.Build(propInfo);
        }

        public static bool IsNullable(this PropertyInfo pi, CsdlPropertyAttribute csdlPropertyAttribute = null)
        {
            var type = pi.PropertyType;
            // Use what is specified in CsdlPropertyAttribute
            csdlPropertyAttribute = csdlPropertyAttribute ?? pi.GetCustomAttribute<CsdlPropertyAttribute>();
            if (csdlPropertyAttribute != null && csdlPropertyAttribute.NullableSet)
                return csdlPropertyAttribute.Nullable;
            // The required Attribute means, NOT nullable
            if (pi?.GetCustomAttribute<RequiredAttribute>() != null)
                return false;
            // Types such as string or Nullable<T> are nullable
            if (type == typeof(string) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return true;
            // Unless specified, ValueTypes are not nullable
            return !type.IsValueType;
        }

        public static bool ExcludeFromMetadata(this PropertyInfo propInfo)
        {
            return propInfo.GetCustomAttributes(true)
                           .Any(a =>
                               (a is CsdlPropertyAttribute && (a as CsdlPropertyAttribute).ExcludeFromMetadata)
                             || a is ExcludeFromMetadataAttribute
                             || a is JsonIgnoreAttribute
                             || a is IgnoreDataMemberAttribute);
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias, CustomPropertyDataFuncs CustomPropertyDataFuncs = null)
        {
            CustomPropertyDataFuncs = CustomPropertyDataFuncs ?? new CustomPropertyDataFuncs(); 
            return new RelatedEntityNavigationPropertyBuilder(CustomPropertyDataFuncs).Build(relatedEntityAttribute, schemaOrAlias);
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias, CustomPropertyDataFuncs CustomPropertyDataFuncs = null)
        {
            CustomPropertyDataFuncs = CustomPropertyDataFuncs ?? new CustomPropertyDataFuncs();
            return new RelatedEntityForeignNavigationPropertyBuilder(CustomPropertyDataFuncs).Build(relatedEntityAttribute, schemaOrAlias);
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias, CustomPropertyDataFuncs CustomPropertyDataFuncs = null)
        {
            CustomPropertyDataFuncs = CustomPropertyDataFuncs ?? new CustomPropertyDataFuncs();
            return new RelatedEntityMappingNavigationPropertyBuilder(CustomPropertyDataFuncs).Build(relatedEntityAttribute, schemaOrAlias);
        }
    }
}