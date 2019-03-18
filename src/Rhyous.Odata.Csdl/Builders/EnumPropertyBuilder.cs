using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class EnumPropertyBuilder : ICsdlBuilder<PropertyInfo, CsdlEnumProperty>
    {
        public EnumPropertyBuilder(IFuncDictionary<Type, MemberInfo> propertyDataAttributeDictionary = null)
        {
        }

        public CsdlEnumProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType.IsGenericType ? propInfo.PropertyType.GetGenericArguments()[0] : propInfo.PropertyType;
            if (!propertyType.IsEnum)
                return null;
            var prop = new CsdlEnumProperty
            {
                UnderlyingType = CsdlTypeDictionary.Instance[propertyType.GetEnumUnderlyingType().FullName],
                CustomData = propertyType.ToDictionary(),
                IsFlags = propertyType.GetCustomAttributes<FlagsAttribute>().Any()
            };
            AddFromAttributes(prop.CustomData, propInfo, PropertyDataAttributeDictionary);
            return prop;
        }

        /// <summary>
        /// Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        internal void AddFromAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi, IDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> attributeActionDictionary)
        {
            if (propertyDictionary == null || mi == null || attributeActionDictionary == null || !attributeActionDictionary.Any())
                return;
            var attribs = mi.GetCustomAttributes(true);
            if (attribs == null)
                return;
            foreach (var attrib in attribs)
            {
                if (attributeActionDictionary.TryGetValue(attrib.GetType(), out Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> action))
                {
                    var propertyList = action.Invoke(mi);
                    foreach (var prop in propertyList)
                    {
                        // Don't add an attribute if already added.
                        if (propertyDictionary.TryGetValue(prop.Key, out object _))
                            continue;
                        propertyDictionary.AddIfNewAndNotNull(prop.Key, prop.Value);
                    }
                }
            }
        }

        internal IFuncDictionary<Type, MemberInfo> PropertyDataAttributeDictionary
        {
            get { return _PropertyDataAttributeDictionary ?? (_PropertyDataAttributeDictionary = new PropertyDataAttributeDictionary()); }
            set { _PropertyDataAttributeDictionary = value; }
        } private IFuncDictionary<Type, MemberInfo> _PropertyDataAttributeDictionary;

    }
}
