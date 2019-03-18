using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityMappingNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityMappingAttribute, CsdlNavigationProperty>
    {
        public CsdlNavigationProperty Build(RelatedEntityMappingAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
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
            navProp.CustomData.Add(Constants.EAFRelatedEntityType, Constants.Mapping);
            var mappingEntityType = $"{schemaOrAlias}.{relatedEntityAttribute.MappingEntity}";
            navProp.CustomData.Add(Constants.EAFMappingEntityType, mappingEntityType);
            return navProp;
        }
    }
}