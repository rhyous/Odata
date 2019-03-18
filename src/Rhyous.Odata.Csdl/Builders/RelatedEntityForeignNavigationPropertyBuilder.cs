﻿using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityForeignNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityForeignAttribute, CsdlNavigationProperty>
    {
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
            navProp.CustomData.Add(Constants.EAFRelatedEntityType, Constants.Foreign);
            return navProp;
        }
    }
}