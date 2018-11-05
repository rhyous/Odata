using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyAttributeDictionary : Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>
    {
        #region Singleton

        private static readonly Lazy<PropertyAttributeDictionary> Lazy = new Lazy<PropertyAttributeDictionary>(() => new PropertyAttributeDictionary());

        public static PropertyAttributeDictionary Instance { get { return Lazy.Value; } }

        internal PropertyAttributeDictionary()
        {
            Add(typeof(RelatedEntityAttribute), GetRelatedEntityProperties);
        }

        #endregion
        
        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityProperties(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var relatedEntityAttribute = mi.GetCustomAttribute<RelatedEntityAttribute>();
            if (relatedEntityAttribute == null)
                return null;
            var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            return new[] { new KeyValuePair<string, object>(relatedEntityName, relatedEntityAttribute.ToNavigationProperty()) };
        }
    }
}