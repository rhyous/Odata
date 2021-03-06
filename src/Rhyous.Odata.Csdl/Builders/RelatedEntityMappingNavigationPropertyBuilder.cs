﻿using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityMappingNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityMappingAttribute, CsdlNavigationProperty>
    {
        private readonly IFuncList<string, string> _CustomPropertyDataFuncs;

        public RelatedEntityMappingNavigationPropertyBuilder(IFuncList<string, string> CustomPropertyDataFuncs)
        {
            _CustomPropertyDataFuncs = CustomPropertyDataFuncs;
        }

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
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.MappingEntityAlias) && relatedEntityAttribute.MappingEntity != relatedEntityAttribute.MappingEntityAlias)
                navProp.CustomData.Add(Constants.EAFMappingEntityAlias, relatedEntityAttribute.MappingEntityAlias);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.EntityAlias) && relatedEntityAttribute.EntityAlias != relatedEntityAttribute.Entity)
                navProp.CustomData.Add(Constants.EAFEntityAlias, relatedEntityAttribute.EntityAlias);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.Filter))
                navProp.CustomData.Add(Constants.OdataFilter, relatedEntityAttribute.Filter);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.Add(Constants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);

            return navProp;
        }
    }
}