using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityAttribute, CsdlNavigationProperty>
    {
        public CsdlNavigationProperty Build(RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = Constants.DefaultSchemaOrAlias)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                Nullable = relatedEntityAttribute.Nullable,
                IsCollection = false, // RelatedEntityAttribute on a property is never a collection.
                ReferentialConstraint = new Dictionary<string, string> { { relatedEntityAttribute.Property, relatedEntityAttribute.ForeignKeyProperty } }
            };
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.Add(Constants.OdataFilter, relatedEntityAttribute.Filter);
            return navProp;
        }
    }
}