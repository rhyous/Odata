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
        public static CsdlEntity ToCsdl(this Type entityType, IEntityBuilder entityBuilder = null)
        {
            entityBuilder = entityBuilder ?? CsdlBuilderFactory.Instance.EntityBuilder;
            return entityBuilder.Build(entityType);
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
    }
}