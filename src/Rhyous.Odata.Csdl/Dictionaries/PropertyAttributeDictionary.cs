using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyAttributeDictionary : ConcurrentDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>
    {
        #region Singleton

        private static readonly Lazy<PropertyAttributeDictionary> Lazy = new Lazy<PropertyAttributeDictionary>(() => new PropertyAttributeDictionary());

        public static PropertyAttributeDictionary Instance { get { return Lazy.Value; } }

        internal PropertyAttributeDictionary()
        {
            GetOrAdd(typeof(RelatedEntityAttribute), (Type t) => { return GetRelatedEntityProperties; });
        }

        #endregion
        
        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityProperties(MemberInfo mi)
        {
            var relatedEntityAttributes = mi?.GetCustomAttributes<RelatedEntityAttribute>()?.GroupBy(a => a.Entity);
            if (relatedEntityAttributes == null || !relatedEntityAttributes.Any())
                return null;
            var navKeyList = new List<KeyValuePair<string, object>>();
            foreach (var group in relatedEntityAttributes)
            {
                var mergedAttribute = group.Merge();
                var relatedEntityName = string.IsNullOrWhiteSpace(mergedAttribute.RelatedEntityAlias) ? mergedAttribute.RelatedEntity : mergedAttribute.RelatedEntityAlias;
                var relatedEntityMetadata = new KeyValuePair<string, object>("@EAF.RelatedEntity.Type", "Local");
                var navProp = mergedAttribute.ToNavigationProperty(CsdlExtensions.DefaultSchemaOrAlias, relatedEntityMetadata);
                navKeyList.Add(new KeyValuePair<string, object>(relatedEntityName, navProp));
            }
            return navKeyList;
        }
    }
}