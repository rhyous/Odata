using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyAttributeDictionary : AttributeFuncDictionary
    {
        #region Constructor

        public PropertyAttributeDictionary()
        {
            Add(typeof(RelatedEntityAttribute), GetRelatedEntityProperties);
        }

        #endregion
        
        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityProperties(MemberInfo mi)
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
                var navProp = mergedAttribute.ToNavigationProperty(CsdlConstants.DefaultSchemaOrAlias);
                navProp.CustomData.Add(CsdlConstants.EAFRelatedEntityType, CsdlConstants.Local);
                navKeyList.Add(new KeyValuePair<string, object>(relatedEntityName, navProp));
            }
            return navKeyList;
        }
    }
}