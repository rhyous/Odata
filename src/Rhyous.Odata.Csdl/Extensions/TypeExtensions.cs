using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Attribute> GetInterfaceAttributesNotOverridden(this Type type, HashSet<Type> overriddenAttributeTypes)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            if (overriddenAttributeTypes == null) { overriddenAttributeTypes = new HashSet<Type>(); }
            return type.GetInterfaceAttributes()
                       .Where(ia => !overriddenAttributeTypes.Contains(ia.GetType()));
        }

        public static IEnumerable<Attribute> GetInterfaceAttributes(this Type type)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }

           return type.GetInterfaces()
                      .SelectMany(i => i.GetCustomAttributes());
        }

        public static IEnumerable<T> GetAttributesWithInterfaceInheritance<T>(this Type type)
            where T : Attribute
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }

            var attribs = type.GetCustomAttributes<T>().ToList();
            if (attribs == null || !attribs.Any())
            {
                var interfaceAttribs = type.GetInterfaceAttributes()
                                           .Where(ia => ia.GetType() == typeof(T))
                                           .Select(ia => ia as T);
                attribs.AddRange(interfaceAttribs);
            }
            return attribs;
        }

        public static T GetAttributeWithInterfaceInheritance<T>(this Type type)
            where T : Attribute
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }

            return type.GetCustomAttribute<T>() ?? 
                   type.GetInterfaceAttributes()
                       .Where(ia => ia.GetType() == typeof(T))
                       .FirstOrDefault() as T;
        }
    }
}
