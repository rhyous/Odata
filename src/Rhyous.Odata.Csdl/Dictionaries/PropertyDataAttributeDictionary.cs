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
            Add(typeof(CsdlPropertyAttribute), HandleCsdPropertyAttribute);
            Add(typeof(MinLengthAttribute), GetMinLengthProperty);
            Add(typeof(MaxLengthAttribute), GetMaxLengthProperty);
            Add(typeof(StringLengthAttribute), GetStringLengthProperty);
            Add(typeof(RangeAttribute), GetRangeProperty);
            Add(typeof(UIHintAttribute), GetUIHintProperty);
        }

        #endregion

        internal IEnumerable<KeyValuePair<string, object>> GetReadOnlyProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var editableAttribute = mi.GetCustomAttribute<EditableAttribute>(true);
            if (editableAttribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIReadOnly, !editableAttribute.AllowEdit) };
        }

        internal IEnumerable<KeyValuePair<string, object>> GetRelatedEntityPropertyData(MemberInfo mi)
        {
            var relatedEntityAttributes = mi?.GetCustomAttributes<RelatedEntityAttribute>() // this never returns null
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

        private IEnumerable<KeyValuePair<string, object>> GetRequiredProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<RequiredAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIRequired, true) };
        }

        private IEnumerable<KeyValuePair<string, object>> HandleCsdPropertyAttribute(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<CsdlPropertyAttribute>(true);
            if (attribute == null)
                return null;
            var data = new List<KeyValuePair<string, object>>();
            if (attribute.ReadOnly) // The default is false, so no reason to include this if it is the default 
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIReadOnly, attribute.ReadOnly));
            if (attribute.RequiredSet) // Make sure the Required property is set
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIRequired, attribute.Required));
            if (!string.IsNullOrWhiteSpace(attribute.UIHint))
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIHint, attribute.UIHint));
            if (attribute.MinLength > 0)
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIMinLength, attribute.MinLength));
            if (attribute.MaxLength > 0)
                data.Add(new KeyValuePair<string, object>(CsdlConstants.UIMaxLength, attribute.MaxLength));
            return data;
        }

        private IEnumerable<KeyValuePair<string, object>> GetMinLengthProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<MinLengthAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIMinLength, attribute.Length) };
        }

        private IEnumerable<KeyValuePair<string, object>> GetMaxLengthProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<MaxLengthAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIMaxLength, attribute.Length) };
        }

        private IEnumerable<KeyValuePair<string, object>> GetStringLengthProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<StringLengthAttribute>(true);
            if (attribute == null)
                return null;
            return new[] 
            { 
                new KeyValuePair<string, object>(CsdlConstants.UIMinLength, attribute.MinimumLength),
                new KeyValuePair<string, object>(CsdlConstants.UIMaxLength, attribute.MaximumLength)
            };
        }

        private IEnumerable<KeyValuePair<string, object>> GetRangeProperty(MemberInfo mi)
        {

            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<RangeAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIRange, new { attribute.Minimum, attribute.Maximum }) };
        }

        private IEnumerable<KeyValuePair<string, object>> GetUIHintProperty(MemberInfo mi)
        {
            if (mi == null)
                return null;
            var attribute = mi.GetCustomAttribute<UIHintAttribute>(true);
            if (attribute == null)
                return null;
            return new[] { new KeyValuePair<string, object>(CsdlConstants.UIMaxLength, attribute.UIHint) };
        }
    }
}