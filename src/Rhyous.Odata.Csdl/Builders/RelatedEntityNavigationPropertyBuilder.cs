using Rhyous.Collections;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class RelatedEntityNavigationPropertyBuilder : ICsdlNavigationPropertyBuilder<RelatedEntityAttribute, CsdlNavigationProperty>
    {
        private readonly IFuncList<string, string> _CustomPropertyDataFuncs;

        public RelatedEntityNavigationPropertyBuilder(IFuncList<string, string> CustomPropertyDataFuncs)
        {
            _CustomPropertyDataFuncs = CustomPropertyDataFuncs;
        }

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
                navProp.CustomData.AddIfNew(Constants.OdataFilter, relatedEntityAttribute.Filter);
            if (!string.IsNullOrWhiteSpace(relatedEntityAttribute.DisplayCondition))
                navProp.CustomData.AddIfNew(Constants.OdataDisplayCondition, relatedEntityAttribute.DisplayCondition);
            if (relatedEntityAttribute.Nullable)
                navProp.CustomData.AddIfNew(Constants.Default, null);
            if (relatedEntityAttribute.AllowedNonExistentValue != null)
            {
                navProp.CustomData.AddOrReplace(Constants.Default, new CsdlNameValue
                {
                    Name = relatedEntityAttribute.AllowedNonExistentValueName,
                    Value = relatedEntityAttribute.AllowedNonExistentValue
                });
            }
            return navProp;
        }
    }
}