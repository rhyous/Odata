using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations inside a property based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyDataAttributeDictionary : Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>
    {
        #region Singleton

        private static readonly Lazy<PropertyDataAttributeDictionary> Lazy = new Lazy<PropertyDataAttributeDictionary>(() => new PropertyDataAttributeDictionary());

        public static PropertyDataAttributeDictionary Instance { get { return Lazy.Value; } }

        internal PropertyDataAttributeDictionary()
        {
            Add(typeof(EditableAttribute), GetReadOnlyProperty);
            Add(typeof(RelatedEntityAttribute), GetRelatedEntityPropertyData);
            // Future: 
            // Add(typeof(MinLengthAttribute), GetMaxLengthProperty);
            // Add(typeof(MaxLengthAttribute), GetMaxLengthProperty);
            // Add(typeof(RangeAttribute), GetMaxLengthProperty);
        }

        #endregion

        internal IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var editableAttribute = mi.GetCustomAttribute<EditableAttribute>(true);
            if (editableAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>("@UI.ReadOnly", !editableAttribute.AllowEdit) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityPropertyData(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var relatedEntityAttribute = mi.GetCustomAttribute<RelatedEntityAttribute>();
            if (relatedEntityAttribute == null)
                return null;
            var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
            var navKey = new KeyValuePair<string, object>("$NavigationKey", relatedEntityName);
            return new [] { navKey };
        }
    }
}