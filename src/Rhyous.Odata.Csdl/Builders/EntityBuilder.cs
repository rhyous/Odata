using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class EntityBuilder : ICsdlBuilder<Type, CsdlEntity>
    {
        private readonly ICsdlBuilder<PropertyInfo, CsdlProperty> _PropertyBuilder;
        private readonly ICsdlBuilder<PropertyInfo, CsdlEnumProperty> _EnumPropertyBuilder;
        private readonly IFuncDictionary<Type, MemberInfo> _PropertyAttributeFuncDictionary;
        private readonly IFuncDictionary<Type, MemberInfo> _EntityAttributeFuncDictionary;

        public EntityBuilder(ICsdlBuilder<PropertyInfo, CsdlProperty> propertyBuilder,
                             ICsdlBuilder<PropertyInfo, CsdlEnumProperty> enumPropertyBuilder, 
                             IFuncDictionary<Type, MemberInfo> entityAttributeFuncDictionary,
                             IFuncDictionary<Type, MemberInfo> propertyAttributeFuncDictionary
                            )
        {
            _PropertyBuilder = propertyBuilder;
            _EnumPropertyBuilder = enumPropertyBuilder;
            _EntityAttributeFuncDictionary = entityAttributeFuncDictionary;
            _PropertyAttributeFuncDictionary = propertyAttributeFuncDictionary;
        }

        /// <summary>
        /// Builds a CsdlEntity from a Type.
        /// </summary>
        /// <param name="entityType">The type.</param>
        /// <param name="customPropertyBuilders">Custom property builders.</param>
        /// <returns>A CsdlEntity.</returns>
        public CsdlEntity Build(Type entityType)
        {
            if (entityType == null)
                return null;
            var entity = new CsdlEntity { Keys = new List<string> { Constants.Id } };
            //if (customPropertyBuilders != null)
            //    entity.Properties.AddCustomProperties(entityType, customPropertyBuilders);
            foreach (var propInfo in entityType.GetProperties().OrderBy(p => p.Name))
            {
                // If the property was added via a customization, don't add it again
                if (entity.Properties.TryGetValue(entityType.Name, out object _))
                    continue; ;
                AddFromPropertyInfo(entity.Properties, propInfo);
                // Add new properties based on this Property's attributes
                entity.Properties.AddFromAttributes(propInfo, _PropertyAttributeFuncDictionary);
            }
            // Add new properties based on this Entity's attributes
            entity.Properties.AddFromAttributes(entityType, _EntityAttributeFuncDictionary);
            return entity;
        }

        internal void AddFromPropertyInfo(IDictionary<string, object> dictionary, PropertyInfo propInfo)
        {
            if (dictionary == null || propInfo == null)
                return;
            if (propInfo.PropertyType.IsEnum)
                dictionary.AddIfNewAndNotNull(propInfo.Name, _EnumPropertyBuilder.Build(propInfo));
            else
                dictionary.AddIfNewAndNotNull(propInfo.Name, _PropertyBuilder.Build(propInfo));
        }
    }
}