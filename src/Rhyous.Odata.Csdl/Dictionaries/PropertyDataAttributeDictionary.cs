using System;
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
    public class PropertyDataAttributeDictionary : AttributeFuncDictionary
    {
        #region Constructor

        public PropertyDataAttributeDictionary()
        {
            Add(typeof(EditableAttribute), GetReadOnlyProperty);
            Add(typeof(RelatedEntityAttribute), GetRelatedEntityPropertyData);
            Add(typeof(RequiredAttribute), GetRequiredProperty);
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
            return new[] { new KeyValuePair<string, object>(Constants.UIReadOnly, !editableAttribute.AllowEdit) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityPropertyData(MemberInfo mi)
        {
            var relatedEntityAttributes = mi?.GetCustomAttributes<RelatedEntityAttribute>() // this never returns null
                                             .GroupBy(a=>a.Entity);
            if (relatedEntityAttributes == null || !relatedEntityAttributes.Any())
                return null;
            var navKeyList = new List<KeyValuePair<string, object>>();
            foreach (var group in relatedEntityAttributes)
            {
                var mergedAttribute = group.Merge();
                var relatedEntity = mergedAttribute.RelatedEntity;
                var relatedEntityAlias = mergedAttribute.RelatedEntityAlias;
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAlias) ? relatedEntity : relatedEntityAlias;
                navKeyList.Add(new KeyValuePair<string, object>(Constants.NavigationKey, relatedEntityName));
                if (mergedAttribute.RelatedEntityMustExist)
                    navKeyList.Add(new KeyValuePair<string, object>(Constants.NavigationKey, relatedEntityName));
            }
            return navKeyList;
        }

        private IEnumerable<KeyValuePair<string, object>> GetRequiredProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<RequiredAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(Constants.UIReqired, true) };
        }
    }
}