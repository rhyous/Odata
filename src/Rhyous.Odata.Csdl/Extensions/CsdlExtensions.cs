using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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

        public static bool IsNullable(this Type type, PropertyInfo pi)
        {
            if (type == typeof(string))
                return pi?.GetCustomAttribute<RequiredAttribute>() == null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            return !type.IsValueType;
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