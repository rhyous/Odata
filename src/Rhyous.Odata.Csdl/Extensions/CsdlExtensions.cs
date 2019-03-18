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
        public static CsdlEntity ToCsdl(this Type entityType)
        {
            var propertyBuilder = new PropertyBuilder(new PropertyDataAttributeDictionary(), new CustomPropertyDataDictionary());
            var entityBuilder = new EntityBuilder(propertyBuilder, new EnumPropertyBuilder(), new EntityAttributeDictionary(), new PropertyAttributeDictionary());
            return entityBuilder.Build(entityType);
        }

        public static CsdlProperty ToCsdl(this PropertyInfo propInfo)
        {

            var propertyBuilder = new PropertyBuilder(new PropertyDataAttributeDictionary(), new CustomPropertyDataDictionary()); return propertyBuilder.Build(propInfo);
        }

        public static CsdlEnumProperty ToCsdlEnum(this PropertyInfo propInfo)
        {

            var propertyBuilder = new PropertyBuilder(new PropertyDataAttributeDictionary(), new CustomPropertyDataDictionary()); return propertyBuilder.BuildEnumProperty(propInfo);
        }

        public static bool IsNullable(this Type type, PropertyInfo pi)
        {
            if (type == typeof(string))
                return pi?.GetCustomAttribute<RequiredAttribute>() == null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return true;
            return !type.IsValueType;
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
        {
            return new RelatedEntityNavigationPropertyBuilder().Build(relatedEntityAttribute, schemaOrAlias);
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
        {
            return new RelatedEntityForeignNavigationPropertyBuilder().Build(relatedEntityAttribute, schemaOrAlias);
        }

        public static CsdlNavigationProperty ToNavigationProperty(this RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
        {
            return new RelatedEntityMappingNavigationPropertyBuilder().Build(relatedEntityAttribute, schemaOrAlias);
        }
    }
}