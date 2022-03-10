using Rhyous.Collections;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    /// <summary>An interface for a CSDL appender, that appends custom CSDL data from attributes.</summary>
    public interface ICustomCsdlFromAttributeAppender
    {
        /// <summary>
        /// Adds property data from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        void AppendPropertyDataFromAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);

        /// <summary>
        /// Adds a property from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        void AppendPropertiesFromEntityAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);

        /// <summary>
        /// Adds a property from an attribute. Type and PropertyInfo both inherit MemberInfo.
        /// </summary>
        void AppendPropertiesFromPropertyAttributes(IConcurrentDictionary<string, object> propertyDictionary, MemberInfo mi);
    }
}