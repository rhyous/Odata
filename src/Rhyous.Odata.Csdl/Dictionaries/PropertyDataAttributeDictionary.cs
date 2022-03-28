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
    public class PropertyDataAttributeDictionary : AttributeFuncDictionary, IPropertyDataAttributeDictionary
    {
        #region Constructor

        public PropertyDataAttributeDictionary()
        {
            GetOrAdd(typeof(EditableAttribute), GetReadOnlyProperty);
            GetOrAdd(typeof(RelatedEntityAttribute), GetRelatedEntityPropertyData);
            GetOrAdd(typeof(RequiredAttribute), GetRequiredProperty);
            GetOrAdd(typeof(CsdlPropertyAttribute), HandleCsdPropertyAttribute);
            GetOrAdd(typeof(CsdlStringPropertyAttribute), HandleCsdStringPropertyAttribute);
            GetOrAdd(typeof(RangeAttribute), GetRangeProperty);
            GetOrAdd(typeof(UIHintAttribute), GetUIHintProperty);
            GetOrAdd(typeof(HRefAttribute), GetHrefProperty);
        }

        #endregion

        internal IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi)
        {
            var pi = mi as PropertyInfo;
            if (pi == null)
                return null;
            var editableAttribute = pi.GetAttributeWithInterfaceInheritance<EditableAttribute>();
            if (editableAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIReadOnly, !editableAttribute.AllowEdit) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityPropertyData(MemberInfo mi)
        {
            var relatedEntityAttributes = mi.GetAttributesWithInterfaceInheritance<RelatedEntityAttribute>() // this never returns null
                                            .GroupBy(a => a.Entity);
            if (relatedEntityAttributes == null || !relatedEntityAttributes.Any())
                return null;
            var navKeyList = new List<KeyValuePair<string, object>>();
            foreach (var group in relatedEntityAttributes)
            {
                var mergedAttribute = group.Merge();
                var relatedEntity = mergedAttribute.RelatedEntity;
                var relatedEntityAlias = mergedAttribute.RelatedEntityAlias;
                var relatedEntityName = string.IsNullOrWhiteSpace(relatedEntityAlias) ? relatedEntity : relatedEntityAlias;
                navKeyList.Add(new KeyValuePair<string, object>(CsdlConstants.NavigationKey, relatedEntityName));
                if (mergedAttribute.RelatedEntityMustExist)
                    navKeyList.Add(new KeyValuePair<string, object>(CsdlConstants.NavigationKey, relatedEntityName));
            }
            return navKeyList;
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRequiredProperty(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<RequiredAttribute>();
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIRequired, true) };
        }

        internal IEnumerable<KeyValuePair<string, object>> HandleCsdPropertyAttribute(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<CsdlPropertyAttribute>();
            if (attribute == null)
                return null;
            var data = new List<KeyValuePair<string, object>>();
            if (attribute.ReadOnly) // The default is false, so no reason to include this if it is the default 
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIReadOnly, attribute.ReadOnly));
            if (attribute.RequiredSet) // Make sure the Required property is set
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIRequired, attribute.Required));
            if (!string.IsNullOrWhiteSpace(attribute.UIHint)) // Add a UI hint
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIHint, attribute.UIHint));
            return data;
        }

        internal IEnumerable<KeyValuePair<string, object>> HandleCsdStringPropertyAttribute(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<CsdlStringPropertyAttribute>();
            if (attribute == null)
                return null;
            var data = new List<KeyValuePair<string, object>>();
            if (!string.IsNullOrWhiteSpace(attribute.StringType)) // Add a UI hint
                data.Add(new KeyValuePair<string, object>(CsdlConstants.StringType, attribute.StringType));
            return data;

        }

        internal IEnumerable<KeyValuePair<string, object>> GetRangeProperty(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<RangeAttribute>();
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIRange, new { attribute.Minimum, attribute.Maximum }) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetUIHintProperty(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<UIHintAttribute>();
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIHint, attribute.UIHint) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetHrefProperty(MemberInfo mi)
        {
            var attribute = mi.GetAttributeWithInterfaceInheritance<HRefAttribute>();
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.StringType, CsdlConstants.Href) };
        }
    }
}