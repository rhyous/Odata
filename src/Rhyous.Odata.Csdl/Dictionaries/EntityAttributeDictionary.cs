using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// Creates additional properties or annotations on the entity based on an entity's attributes.
    /// </summary>
    /// <remarks>Try to use attributes from System.ComponentModel.DataAnnotations before creating new ones.</remarks>

    public class EntityAttributeDictionary : Dictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>
    {
        #region Singleton

        private static readonly Lazy<EntityAttributeDictionary> Lazy = new Lazy<EntityAttributeDictionary>(() => new EntityAttributeDictionary());

        public static EntityAttributeDictionary Instance { get { return Lazy.Value; } }

        internal EntityAttributeDictionary()
        {
            Add(typeof(DisplayColumnAttribute), GetDisplayProperty);
            Add(typeof(ReadOnlyEntityAttribute), GetReadOnlyProperty);
            Add(typeof(RelatedEntityForeignAttribute), GetRelatedEntityForeignProperties);
            Add(typeof(RelatedEntityMappingAttribute), GetRelatedEntityMappingProperties);
        }

        #endregion
        
        internal IEnumerable<KeyValuePair<string, object>> GetDisplayProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var displayColumnAttribute = mi.GetCustomAttribute<DisplayColumnAttribute>(true);
            if (displayColumnAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>("@UI.DisplayProperty", displayColumnAttribute.DisplayColumn) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var editableAttribute = mi.GetCustomAttribute<ReadOnlyEntityAttribute>(true);
            if (editableAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>("@UI.ReadOnly", true) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityForeignProperties(MemberInfo mi)
        {
            if (mi == null)
                yield break;
            foreach (var relatedEntityAttribute in mi.GetCustomAttributes<RelatedEntityForeignAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                yield return new KeyValuePair<string, object>(relatedEntityName, relatedEntityAttribute.ToNavigationProperty());
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityMappingProperties(MemberInfo mi)
        {
            if (mi == null)
                yield break;
            foreach (var relatedEntityAttribute in mi.GetCustomAttributes<RelatedEntityMappingAttribute>(true))
            {
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAttribute.RelatedEntityAlias) ? relatedEntityAttribute.RelatedEntity : relatedEntityAttribute.RelatedEntityAlias;
                yield return new KeyValuePair<string, object>(relatedEntityName, relatedEntityAttribute.ToNavigationProperty());
            }
        }
    }
}