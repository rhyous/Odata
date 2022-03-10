using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>An attribute that marks a property to be exlcluded from meteadata.</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromMetadataAttribute : Attribute
    {
    }
}
