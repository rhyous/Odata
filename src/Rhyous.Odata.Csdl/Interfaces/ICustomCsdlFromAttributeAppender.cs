using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomCsdlFromAttributeAppender
    {
        void AppendPropertyDataFromAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi);
        void AppendPropertiesFromEntityAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi);
        void AppendPropertiesFromPropertyAttributes(IDictionary<string, object> propertyDictionary, MemberInfo mi);
    }
}