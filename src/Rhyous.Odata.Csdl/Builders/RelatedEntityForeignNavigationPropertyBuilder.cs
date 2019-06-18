using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityForeignNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityForeignAttribute, CsdlNavigationProperty>
    {
        private readonly IFuncList<string, string> _CustomPropertyDataFuncs;

        public RelatedEntityForeignNavigationPropertyBuilder(IFuncList<string, string> CustomPropertyDataFuncs)
        {
            _CustomPropertyDataFuncs = CustomPropertyDataFuncs;
        }

        public CsdlNavigationProperty Build(RelatedEntityForeignAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                IsCollection = true, // RelatedEntityForeignAttribute is always a collection.
                Nullable = true    // Collections can always be empty
            };
            navProp.CustomData.AddFromCustomDictionary(relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity, _CustomPropertyDataFuncs);
            navProp.CustomData.AddFromCustomDictionary(relatedEntityAttribute.Entity, relatedEntityAttribute.RelatedEntity, _CustomPropertyDataFuncs);
            navProp.CustomData.Add(Constants.EAFRelatedEntityType, Constants.Foreign);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.ForeignKeyProperty) && relatedEntityAttribute.ForeignKeyProperty != relatedEntityAttribute.RelatedEntity + Constants.Id)
                navProp.CustomData.Add(Constants.EAFRelatedEntityForeignKeyProperty, relatedEntityAttribute.ForeignKeyProperty);
            return navProp;
        }
    }
}