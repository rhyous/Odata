using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations inside a property based on a property's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>
    public class PropertyDataAttributeDictionary : ConcurrentDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>
    {
        #region Singleton

        private static readonly Lazy<PropertyDataAttributeDictionary> Lazy = new Lazy<PropertyDataAttributeDictionary>(() => new PropertyDataAttributeDictionary());

        public static PropertyDataAttributeDictionary Instance { get { return Lazy.Value; } }

        internal PropertyDataAttributeDictionary()
        {
            GetOrAdd(typeof(EditableAttribute), (Type t) => { return GetReadOnlyProperty; });
            GetOrAdd(typeof(RelatedEntityAttribute), (Type t) => { return GetRelatedEntityPropertyData; });
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
            var relatedEntityAttributes = mi.GetCustomAttributes<RelatedEntityAttribute>().GroupBy(a=>a.Entity);
            if (relatedEntityAttributes == null || !relatedEntityAttributes.Any())
                return null;
            var navKeyList = new List<KeyValuePair<string, object>>();
            foreach (var group in relatedEntityAttributes)
            {
                var relatedEntity = group.FirstOrDefault().RelatedEntity;
                var relatedEntityAlias = group.FirstOrDefault(a => a.RelatedEntityAlias != null)?.RelatedEntityAlias;
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAlias) ? relatedEntity : relatedEntityAlias;
                navKeyList.Add(new KeyValuePair<string, object>("$NavigationKey", relatedEntityName));
            }
            return navKeyList;
        }
    }
}