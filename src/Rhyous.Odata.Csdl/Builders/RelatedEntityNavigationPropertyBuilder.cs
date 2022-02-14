using Rhyous.Collections;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityNavigationPropertyBuilder : IRelatedEntityNavigationPropertyBuilder
    {
        public RelatedEntityNavigationPropertyBuilder()
        {
        }

        public CsdlNavigationProperty Build(RelatedEntityAttribute relatedEntityAttribute, string schemaOrAlias = CsdlConstants.DefaultSchemaOrAlias)
        {
            if (relatedEntityAttribute == null)
                return null;
            var navProp = new CsdlNavigationProperty
            {
                Type = $"{schemaOrAlias}.{relatedEntityAttribute.RelatedEntity}",
                Nullable = relatedEntityAttribute.Nullable,
                IsCollection = false, // RelatedEntityAttribute on a property is never a collection.
                ReferentialConstraint = new CsdlReferentialConstraint
                {
                    LocalProperty = relatedEntityAttribute.Property,
                    ForeignProperty = relatedEntityAttribute.ForeignKeyProperty,
                    CustomData = new Dictionary<string, object> { { relatedEntityAttribute.Property, relatedEntityAttribute.ForeignKeyProperty } }
                }
            };
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.AddIfNew(CsdlConstants.OdataFilter, relatedEntityAttribute.Filter);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.AddIfNew(CsdlConstants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);
            if (relatedEntityAttribute.Nullable)
                navProp.CustomData.AddIfNew(CsdlConstants.Default, null);
            if (relatedEntityAttribute.AllowedNonExistentValue != null)
            {
                navProp.CustomData.AddOrReplace(CsdlConstants.Default, new CsdlNameValue
                {
                    Name = relatedEntityAttribute.AllowedNonExistentValueName,
                    Value = relatedEntityAttribute.AllowedNonExistentValue
                });
            }
            return navProp;
        }
    }
}