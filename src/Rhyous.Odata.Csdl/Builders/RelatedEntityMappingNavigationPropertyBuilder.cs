using Rhyous.Collections;
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
            navProp.CustomData.TryAdd(CsdlConstants.EAFRelatedEntityType, CsdlConstants.Mapping);
            var mappingEntityType = $"{schemaOrAlias}.{relatedEntityAttribute.MappingEntity}";
            navProp.CustomData.TryAdd(CsdlConstants.EAFMappingEntityType, mappingEntityType);

            navProp.AddBaseRelatedEntityPropertyData(relatedEntityAttribute, schemaOrAlias);

            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.MappingEntityAlias) && relatedEntityAttribute.MappingEntity != relatedEntityAttribute.MappingEntityAlias)
                navProp.CustomData.TryAdd(CsdlConstants.EAFMappingEntityAlias, relatedEntityAttribute.MappingEntityAlias);

            return navProp;
        }
    }
}