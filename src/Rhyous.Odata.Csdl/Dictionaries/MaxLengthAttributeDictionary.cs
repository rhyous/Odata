using Rhyous.Collections;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class MaxLengthAttributeDictionary : ConcurrentDictionaryWrapper<Type, Func<Attribute, ulong>>, IMaxLengthAttributeDictionary
    {
        public MaxLengthAttributeDictionary()
        {
            GetOrAdd(typeof(CsdlPropertyAttribute), GetFromCsdlPropertyAttribute);
            GetOrAdd(typeof(MaxLengthAttribute), GetFromMaxLengthAttribute);
            GetOrAdd(typeof(StringLengthAttribute), GetFromStringLengthAttribute);
        }

        public ulong GetMaxLength(PropertyInfo pi)
        {
            // Look for an attribute on the concrete class's property first
            var attribs = pi.GetCustomAttributes()
                            .Where(a => Keys.Contains(a.GetType()));

            // Look for an attribute using inheritance
            if (!attribs.Any())
                attribs = pi.GetCustomAttributes(true)
                            .Where(a => Keys.Contains(a.GetType()))
                            .Select(a => a as Attribute);

            // If no attribute is on the concrete class or in class inheritance,
            // check if there is an attribute on the interface's property
            if (!attribs.Any())
                attribs = pi.GetAttributesWithInterfaceInheritance()
                            .Where(a => Keys.Contains(a.GetType()));

            if (!attribs.Any())
                return 0;

            // Loop through the Attributes if there are more than one and get the lowest
            var minMaxLengthGreaterThanZero = ulong.MaxValue;
            foreach (var attrib in attribs)
            {
                if (TryGetValue(attrib.GetType(), out var func))
                {
                    var attribMax = func.Invoke(attrib);
                    if (func.Invoke(attrib) > 0)
                        minMaxLengthGreaterThanZero = Math.Min(minMaxLengthGreaterThanZero, attribMax);
                }
            }
            return minMaxLengthGreaterThanZero;
        }

        internal ulong GetFromCsdlPropertyAttribute(Attribute a)
        {
            return a is CsdlPropertyAttribute attrib
                 ? attrib.MaxLength
                 : 0;
        }

        internal ulong GetFromMaxLengthAttribute(Attribute a)
        {
            return a is MaxLengthAttribute attrib
                 ? (ulong)attrib.Length
                 : 0;
        }

        internal ulong GetFromStringLengthAttribute(Attribute a)
        {
            return a is StringLengthAttribute attrib
                 ? (ulong)attrib.MaximumLength
                 : 0;
        }
    }
}