using Rhyous.StringLibrary.Pluralization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on an entity's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>

    public class EntityAttributeDictionary : AttributeFuncDictionary, IEntityAttributeDictionary
    {
        private readonly IRelatedEntityForeignNavigationPropertyBuilder _RelatedEntityForeignNavigationPropertyBuilder;
        private readonly IRelatedEntityMappingNavigationPropertyBuilder _RelatedEntityMappingNavigationPropertyBuilder;
        #region Constructor

        public EntityAttributeDictionary(IRelatedEntityForeignNavigationPropertyBuilder relatedEntityForeignNavigationPropertyBuilder,
                                         IRelatedEntityMappingNavigationPropertyBuilder relatedEntityMappingNavigationPropertyBuilder)
        {
            _RelatedEntityForeignNavigationPropertyBuilder = relatedEntityForeignNavigationPropertyBuilder;
            _RelatedEntityMappingNavigationPropertyBuilder = relatedEntityMappingNavigationPropertyBuilder;
            GetOrAdd(typeof(DisplayColumnAttribute), GetDisplayProperty);
            GetOrAdd(typeof(ReadOnlyEntityAttribute), GetReadOnlyProperty);
            GetOrAdd(typeof(RequiredAttribute), GetRequiredProperty);
            GetOrAdd(typeof(RelatedEntityForeignAttribute), GetRelatedEntityForeignProperties);
            GetOrAdd(typeof(RelatedEntityMappingAttribute), GetRelatedEntityMappingProperties);
            GetOrAdd(typeof(MappingEntityAttribute), GetMappingEntityProperties);
            GetOrAdd(typeof(LookupEntityAttribute), GetLookupEntityProperties);
        }

        #endregion

        public IEnumerable<KeyValuePair<string, object>> GetDisplayProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var displayColumnAttribute = mi.GetCustomAttribute<DisplayColumnAttribute>(true);
            if (displayColumnAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIDisplayProperty, displayColumnAttribute.DisplayColumn) };
        }

        public IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var editableAttribute = mi.GetCustomAttribute<ReadOnlyEntityAttribute>(true);
            if (editableAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIReadOnly, true) };
        }

        public IEnumerable<KeyValuePair<string, object>> GetRequiredProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var requiredAttribute = mi.GetCustomAttribute<RequiredAttribute>(true);
            if (requiredAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIRequired, true) };
        }

        public IEnumerable<KeyValuePair<string, object>> GetRelatedEntityForeignProperties(MemberInfo mi)
        {
            if (mi == null)
                yield break;
            foreach (var relatedEntityAttribute in mi.GetCustomAttributes<RelatedEntityForeignAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                var pluralizedRelatedEntityName = relatedEntityName.Pluralize();
                yield return new KeyValuePair<string, object>(pluralizedRelatedEntityName, _RelatedEntityForeignNavigationPropertyBuilder.Build(relatedEntityAttribute));
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetRelatedEntityMappingProperties(MemberInfo mi)
        {
            if (mi == null)
                yield break;
            foreach (var relatedEntityAttribute in mi.GetCustomAttributes<RelatedEntityMappingAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                var pluralizedRelatedEntityName = relatedEntityName.Pluralize();
                yield return new KeyValuePair<string, object>(pluralizedRelatedEntityName, _RelatedEntityMappingNavigationPropertyBuilder.Build(relatedEntityAttribute));
            }
        }

        public IEnumerable<KeyValuePair<string, object>> GetMappingEntityProperties(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<MappingEntityAttribute>(true);
            if (attribute == null)
                return null;
            var entity1NameAndAlias = new MappedEntity
            {
                Name = attribute.Entity1,
                Alias = attribute.Entity1Alias,
                MappingProperty = attribute.Entity1MappingProperty
            };
            var entity2NameAndAlias = new MappedEntity
            {
                Name = attribute.Entity2,
                Alias = attribute.Entity2Alias,
                MappingProperty = attribute.Entity2MappingProperty
            };
            return new[]
            {
                new KeyValuePair<string, object>(CsdlConstants.EafEntityType, CsdlConstants.Mapping),
                new KeyValuePair<string, object>(CsdlConstants.EAFMappedEntity1, entity1NameAndAlias),
                new KeyValuePair<string, object>(CsdlConstants.EAFMappedEntity2, entity2NameAndAlias),
            };
        }
        public IEnumerable<KeyValuePair<string, object>> GetLookupEntityProperties(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<LookupEntityAttribute>(true);
            if (attribute == null)
                return null;
            return new[]
            {
                new KeyValuePair<string, object>(CsdlConstants.EafEntityType, CsdlConstants.Lookup),
                new KeyValuePair<string, object>(CsdlConstants.UIMaxCountToBehaveAsEnum, attribute.MaxCountToBehaveAsEnum)
            };
        }
    }
}