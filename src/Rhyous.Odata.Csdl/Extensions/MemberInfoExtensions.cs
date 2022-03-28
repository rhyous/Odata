using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    internal static class MemberInfoExtensions
    {
        /// <summary>Gets attributes from the class and an interface, unless the class attribute overrides the interface attribute.</summary>
        /// <param name="mi">The MemberInfo: Type or PropInfo. MethodInfo not yet supported.</param>
        /// <returns>Sttributes from the class and an interface, unless the class attribute overrides the interface attribute, then the 
        /// interface attribute is excluded.</returns>
        public static List<Attribute> GetAttributesWithInterfaceInheritance(this MemberInfo mi)
        {
            if (mi is null) { throw new ArgumentNullException(nameof(mi)); }

            var attribs = mi.GetCustomAttributes(true).Select(o => o as Attribute).ToList();
            var distinctAttribTypes = attribs.Select(a => a.GetType()).Distinct();
            var overriddenAttributeTypes = new HashSet<Type>(distinctAttribTypes);

            // Support interface inheritance - C# doesn't support it by default
            if (mi is Type type)
                attribs.AddRange(type.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes));
            if (mi is PropertyInfo pi)
                attribs.AddRange(pi.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes));
            return attribs;
        }

        public static IEnumerable<T> GetAttributesWithInterfaceInheritance<T>(this MemberInfo mi)
            where T : Attribute
        {
            if (mi is Type type)
                return type.GetAttributesWithInterfaceInheritance<T>();
            if (mi is PropertyInfo pi)
                return pi.GetAttributesWithInterfaceInheritance<T>();
            return Enumerable.Empty<T>();
        }

        public static T GetAttributeWithInterfaceInheritance<T>(this MemberInfo mi)
            where T : Attribute
        {
            if (mi is Type type)
                return type.GetAttributeWithInterfaceInheritance<T>();
            if (mi is PropertyInfo pi)
                return pi.GetAttributeWithInterfaceInheritance<T>();
            return null;
        }
    }
}