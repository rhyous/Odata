using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    internal static class PropertyInfoExtensions
    {
        public static IEnumerable<Attribute> GetInterfaceAttributesNotOverridden(this PropertyInfo propInfo, HashSet<Type> overriddenAttributeTypes)
        {
            if (overriddenAttributeTypes == null) { overriddenAttributeTypes = new HashSet<Type>(); }

            return propInfo.GetInterfaceAttributes()
                           .Where(a => !overriddenAttributeTypes.Contains(a.GetType()));
        }

        public static IEnumerable<Attribute> GetInterfaceAttributes(this PropertyInfo propInfo)
        {
            if (propInfo is null) { throw new ArgumentNullException(nameof(propInfo)); }
            var interfacePropInfo = propInfo.GetInterfacePropertyInfo();
            return interfacePropInfo == null 
                 ? Enumerable.Empty<Attribute>() 
                 : interfacePropInfo.GetCustomAttributes();
        }

        public static PropertyInfo GetInterfacePropertyInfo(this PropertyInfo propInfo)
        {
            if (propInfo is null) { throw new ArgumentNullException(nameof(propInfo)); }

            var objType = propInfo.DeclaringType;
            var interfaces = objType.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
                var iPropInfo = interfaceType.GetProperty(propInfo.Name);
                if (iPropInfo != null)
                    return iPropInfo;
            }
            return null;
        }

        public static IEnumerable<T> GetAttributesWithInterfaceInheritance<T>(this PropertyInfo propInfo)
            where T : Attribute
        {
            var attribs = propInfo.GetCustomAttributes<T>(true);
            if (attribs == null)
                attribs = propInfo.GetInterfaceAttributes()
                                  .Where(ia => ia.GetType() == typeof(T))
                                  .Select(ia => ia as T);
            return attribs;
        }

        public static T GetAttributeWithInterfaceInheritance<T>(this PropertyInfo propInfo)
            where T : Attribute
        {
            var attrib = propInfo.GetCustomAttribute<T>(true);
            if (attrib == null)
                attrib = propInfo.GetInterfaceAttributes()
                                  .Where(ia => ia.GetType() == typeof(T))
                                  .FirstOrDefault() as T;
            return attrib;
        }
    }
}