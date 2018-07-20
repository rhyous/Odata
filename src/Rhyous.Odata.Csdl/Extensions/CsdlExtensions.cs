using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlExtensions
    {
        private const string Enum = "System.Enum";

        public static CsdlEntity<TEntity> ToCsdl<TEntity>(this Type entityType)
        {
            var entity = new CsdlEntity<TEntity> { Keys = new List<string> { "Id" } };
            foreach (var property in entityType.GetProperties())
            {
                var csdlProperty = property.ToCsdl();
                if (csdlProperty != null)
                    entity.Properties.Add(csdlProperty);
            }
            return entity;
        }

        private static CsdlProperty ToCsdl(this PropertyInfo property)
        {
            var isNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            var propertyType = isNullable ? property.PropertyType.GetGenericArguments()[0] : property.PropertyType;

            List<string> csdlTypes = new List<string>();
            if (propertyType.FullName != null && CsdlTypeDictionary.Instance.ContainsKey(propertyType.FullName))
            {
                csdlTypes.Add(CsdlTypeDictionary.Instance[propertyType.FullName]);
                if (isNullable)
                    csdlTypes.Add("null");
                return new CsdlProperty
                {
                    Name = property.Name,
                    CsdlType = csdlTypes,
                    CsdlFormat = CsdlFormatDictionary.Instance[propertyType.FullName]
                };
            }
            if (propertyType.IsEnum)
            {
                csdlTypes.Add(CsdlTypeDictionary.Instance[Enum]);
                if (isNullable)
                    csdlTypes.Add("null");
                var underlyingType = CsdlTypeDictionary.Instance[propertyType.GetEnumUnderlyingType().FullName];
                return new CsdlEnumProperty
                {
                    Name = property.Name,
                    CsdlType = csdlTypes,
                    CsdlFormat = CsdlFormatDictionary.Instance[Enum],
                    UnderlyingType = underlyingType
                };
            }
            return null;
        }
    }
}
