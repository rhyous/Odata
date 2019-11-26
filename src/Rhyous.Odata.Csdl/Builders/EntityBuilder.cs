using Newtonsoft.Json;
using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    public class EntityBuilder : ICsdlBuilder<Type, CsdlEntity>
    {
        private readonly ICsdlBuilder<PropertyInfo, CsdlProperty> _PropertyBuilder;
        private readonly ICsdlBuilder<PropertyInfo, CsdlEnumProperty> _EnumPropertyBuilder;
        private readonly IFuncDictionary<Type, MemberInfo> _EntityAttributeFuncDictionary;
        private readonly IFuncDictionary<Type, MemberInfo> _PropertyAttributeFuncDictionary;
        private readonly IFuncList<string> _CustomPropertyFuncs;

        public EntityBuilder(ICsdlBuilder<PropertyInfo, CsdlProperty> propertyBuilder,
                             ICsdlBuilder<PropertyInfo, CsdlEnumProperty> enumPropertyBuilder, 
                             IFuncDictionary<Type, MemberInfo> entityAttributeFuncDictionary,
                             IFuncDictionary<Type, MemberInfo> propertyAttributeFuncDictionary,
                             IFuncList<string> customPropertyFuncs
                            )
        {
            _PropertyBuilder = propertyBuilder;
            _EnumPropertyBuilder = enumPropertyBuilder;
            _EntityAttributeFuncDictionary = entityAttributeFuncDictionary;
            _PropertyAttributeFuncDictionary = propertyAttributeFuncDictionary;
            _CustomPropertyFuncs = customPropertyFuncs;
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
            // Add the Properties based on this Entity's properties.
            foreach (var propInfo in entityType.GetProperties().OrderBy(p => p.Name))
            {
                // If property should be excluded from Metadata, don't include it.
                if (propInfo.GetCustomAttributes(true).Any(a => a is ExcludeFromMetadataAttribute || a is JsonIgnoreAttribute || a is IgnoreDataMemberAttribute))
                    continue;
                // Add a property based on this PropertyInfo.
                AddFromPropertyInfo(entity.Properties, propInfo);
                // Add new properties based on this Property's attributes
                entity.Properties.AddFromAttributes(propInfo, _PropertyAttributeFuncDictionary);
            }
            // Add new properties based on this Entity's attributes
            entity.Properties.AddFromCustomDictionary(entityType.Name, _CustomPropertyFuncs);
            entity.Properties.AddFromAttributes(entityType, _EntityAttributeFuncDictionary);
            return entity;
        }

        internal void AddFromPropertyInfo(IDictionary<string, object> dictionary, PropertyInfo propInfo)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            if (propInfo.PropertyType.IsEnum)
                dictionary.AddIfNewAndNotNull(propInfo.Name, _EnumPropertyBuilder.Build(propInfo));
            else
                dictionary.AddIfNewAndNotNull(propInfo.Name, _PropertyBuilder.Build(propInfo));
        }
    }
}