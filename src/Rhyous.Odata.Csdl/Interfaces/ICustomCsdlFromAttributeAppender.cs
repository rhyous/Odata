using Rhyous.Collections;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomCsdlFromAttributeAppender
    {
        void AppendPropertyDataFromAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);
        void AppendPropertiesFromEntityAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);
        void AppendPropertiesFromPropertyAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);
    }
}