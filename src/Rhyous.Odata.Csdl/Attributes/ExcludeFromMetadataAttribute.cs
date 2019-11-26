using System;

namespace Rhyous.Odata.Csdl
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludeFromMetadataAttribute : Attribute
    {
    }
}
