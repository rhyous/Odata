using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class CustomCsdlFromAttributeAppender : ICustomCsdlFromAttributeAppender
    {
        private readonly IEntityAttributeDictionary _EntityAttributeDictionary;
        private readonly IPropertyAttributeDictionary _PropertyAttributeDictionary;
        private readonly IPropertyDataAttributeDictionary _PropertyDataAttributeDictionary;

        public CustomCsdlFromAttributeAppender(IEntityAttributeDictionary entityAttributeDictionary,
                                 IPropertyAttributeDictionary propertyAttributeDictionary,
                                 IPropertyDataAttributeDictionary propertyDataAttributeDictionary)
        {
            _EntityAttributeDictionary = entityAttributeDictionary;
            _PropertyAttributeDictionary = propertyAttributeDictionary;
            _PropertyDataAttributeDictionary = propertyDataAttributeDictionary;
        }

        

        /// <summary>
        /// Adds property data from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        public void AppendPropertyDataFromAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi)
        {
            AppendFromAttributes(propertyDictionary, mi, _PropertyDataAttributeDictionary);
        }

        /// <summary>
        /// Adds a property from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        public void AppendPropertiesFromEntityAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi)
        {
            AppendFromAttributes(propertyDictionary, mi, _EntityAttributeDictionary);
        }

        /// <summary>
        /// Adds a property from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        public void AppendPropertiesFromPropertyAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi)
        {
            AppendFromAttributes(propertyDictionary, mi, _PropertyAttributeDictionary);
        }

        private void AppendFromAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi, IFuncDictionary<Type, MemberInfo> attributeFuncDictionary)
        {
            if (propertyDictionary == null || mi == null || attributeFuncDictionary == null || !attributeFuncDictionary.Any())
                return;
            var attribs = mi.GetCustomAttributes(true);
            if (attribs == null)
                return;
            foreach (var attrib in attribs)
            {
                if (attributeFuncDictionary.TryGetValue(attrib.GetType(), out Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> action))
                {
                    var propertyList = action(mi);
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
    }
}