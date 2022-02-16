using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityMappingNavigationPropertyBuilder : IRelatedEntityMappingNavigationPropertyBuilder
    {

        public RelatedEntityMappingNavigationPropertyBuilder()
        {
        }

        public CsdlNavigationProperty Build(RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = CsdlConstants.DefaultSchemaOrAlias)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true      // Collections can always be empty
            };
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) && relatedEntityAttribute.RelatedEntity != relatedEntityAttribute.RelatedEntityAlias)
                navProp.Alias = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntityAlias}";
            navProp.CustomData.TryAdd(CsdlConstants.EAFRelatedEntityType, CsdlConstants.Mapping);
            var mappingEntityType = $"{schemaOrAlias}.{relatedEntityAttribute.MappingEntity}";
            navProp.CustomData.TryAdd(CsdlConstants.EAFMappingEntityType, mappingEntityType);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.MappingEntityAlias) && relatedEntityAttribute.MappingEntity != relatedEntityAttribute.MappingEntityAlias)
                navProp.CustomData.TryAdd(CsdlConstants.EAFMappingEntityAlias, relatedEntityAttribute.MappingEntityAlias);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) && relatedEntityAttribute.EntityAlias != relatedEntityAttribute.Entity)
                navProp.CustomData.TryAdd(CsdlConstants.EAFEntityAlias, relatedEntityAttribute.EntityAlias);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.TryAdd(CsdlConstants.OdataFilter, relatedEntityAttribute.Filter);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.TryAdd(CsdlConstants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);

            return navProp;
        }
    }
}