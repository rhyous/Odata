using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityForeignNavigationPropertyBuilder : IRelatedEntityForeignNavigationPropertyBuilder
    {
        private readonly ICustomPropertyDataAppender _CustomPropertyDataAppender;

        public RelatedEntityForeignNavigationPropertyBuilder(ICustomPropertyDataAppender customPropertyDataAppender)
        {
            _CustomPropertyDataAppender = customPropertyDataAppender;
        }

        public CsdlNavigationProperty Build(RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = CsdlConstants.DefaultSchemaOrAlias)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            _CustomPropertyDataAppender.Append(navProp.CustomData, relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity);
            navProp.CustomData.Add(CsdlConstants.EAFRelatedEntityType, CsdlConstants.Foreign);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.ForeignKeyProperty) && relatedEntityAttribute.ForeignKeyProperty != relatedEntityAttribute.Entity + CsdlConstants.Id)
                navProp.CustomData.Add(CsdlConstants.EAFRelatedEntityForeignKeyProperty, relatedEntityAttribute.ForeignKeyProperty);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.Add(CsdlConstants.OdataFilter, relatedEntityAttribute.Filter);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.Add(CsdlConstants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);

            return navProp;
        }
    }
}