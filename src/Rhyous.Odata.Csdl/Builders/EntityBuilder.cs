using Newtonsoft.Json;
using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class EntityBuilder : IEntityBuilder
    {
        private readonly IPropertyBuilder _PropertyBuilder;
        private readonly IEnumPropertyBuilder _EnumPropertyBuilder;
        private readonly ICustomCsdlFromAttributeAppender _CustomCsdlFromAttributeAppender;
        private readonly ICustomPropertyAppender _CustomerPropertyAppender;

        public EntityBuilder(IPropertyBuilder propertyBuilder,
                             IEnumPropertyBuilder enumPropertyBuilder,
                             ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                             ICustomPropertyAppender customerPropertyAppender)
        {
            _PropertyBuilder = propertyBuilder;
            _EnumPropertyBuilder = enumPropertyBuilder;
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            _CustomerPropertyAppender = customerPropertyAppender;
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
            var entity = new CsdlEntity { Keys = new List<string> { CsdlConstants.Id } };
            // Add the Properties based on this Entity's properties.
            foreach (var propInfo in entityType.GetProperties().OrderBy(p => p.Name))
            {
                // If property should be excluded from Metadata, don't include it.
                if (propInfo.ExcludeFromMetadata())
                    continue;
                // Add a property based on this PropertyInfo.
                AddFromPropertyInfo(entity.Properties, propInfo);
                // Add new properties based on this Property's attributes
                _CustomCsdlFromAttributeAppender.AppendPropertiesFromPropertyAttributes(entity.Properties, propInfo);
            }
            // Add new properties based on this Entity's attributes
            _CustomerPropertyAppender.Append(entity.Properties, entityType.Name);
            _CustomCsdlFromAttributeAppender.AppendPropertiesFromEntityAttributes(entity.Properties, entityType);
            return entity;
        }

        internal void AddFromPropertyInfo(IConcurrentDictionary<string, object> dictionary, PropertyInfo propInfo)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            if (propInfo.PropertyType.IsEnum)
                dictionary.TryAddIfNotNull(propInfo.Name, _EnumPropertyBuilder.Build(propInfo));
            else
                dictionary.TryAddIfNotNull(propInfo.Name, _PropertyBuilder.Build(propInfo));
        }
    }
}