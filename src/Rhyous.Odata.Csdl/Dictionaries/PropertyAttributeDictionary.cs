using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyAttributeDictionary : AttributeFuncDictionary, IPropertyAttributeDictionary
    {
        private readonly IRelatedEntityNavigationPropertyBuilder _RelatedEntityNavigationPropertyBuilder;
        #region Constructor

        public PropertyAttributeDictionary(IRelatedEntityNavigationPropertyBuilder relatedEntityNavigationPropertyBuilder)
        {
            _RelatedEntityNavigationPropertyBuilder = relatedEntityNavigationPropertyBuilder;
            GetOrAdd(typeof(RelatedEntityAttribute), GetRelatedEntityProperties);
        }

        #endregion

        public IEnumerable<KeyValuePair<string, object>> GetRelatedEntityProperties(MemberInfo mi)
        {
            var relatedEntityAttributes = mi?.GetCustomAttributes<RelatedEntityAttribute>() // This method is never null
                                             .GroupBy(a => a.Entity);
            if (relatedEntityAttributes == null || !relatedEntityAttributes.Any())
                return null;
            var navKeyList = new List<KeyValuePair<string, object>>();
            foreach (var group in relatedEntityAttributes)
            {
                var mergedAttribute = group.Merge();
                var relatedEntityName = string.IsNullOrWhiteSpace(mergedAttribute.RelatedEntityAlias) ? mergedAttribute.RelatedEntity : mergedAttribute.RelatedEntityAlias;
                var navProp = _RelatedEntityNavigationPropertyBuilder.Build(mergedAttribute, CsdlConstants.DefaultSchemaOrAlias);
                navProp.CustomData.Add(CsdlConstants.EAFRelatedEntityType, CsdlConstants.Local);
                navKeyList.Add(new KeyValuePair<string, object>(relatedEntityName, navProp));
            }
            return navKeyList;
        }
    }
}