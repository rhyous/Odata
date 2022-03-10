using Rhyous.Collections;

namespace Rhyous.Odata.Csdl
{
    public static class CsdlNavigationPropertyExtensions 
    { 
        public static void AddBaseRelatedEntityPropertyData(this CsdlNavigationProperty navProp, RelatedEntityBaseAttribute relatedEntityAttribute, string schemaOrAlias)
        {
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) && relatedEntityAttribute.RelatedEntity != relatedEntityAttribute.RelatedEntityAlias)
                navProp.Alias = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntityAlias}";

            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) && relatedEntityAttribute.EntityAlias != relatedEntityAttribute.Entity)
                navProp.CustomData.TryAdd(CsdlConstants.EAFEntityAlias, relatedEntityAttribute.EntityAlias);

            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.TryAddIfNotNull(CsdlConstants.OdataFilter, relatedEntityAttribute.Filter);

            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.AddIfNew(CsdlConstants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);
        }
    }
}