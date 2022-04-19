using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// An attribute to help configure csdl for a string.
    /// </summary>
    /// <remarks>This attribut replaces the ReadonlyAttribute and ExcludeFromMetadataAttribute</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsdlStringPropertyAttribute : CsdlPropertyAttribute
    {
        public string StringType { get; set; }
    }
}