using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// An attribute to help configure csdl for an integer or number without decimal points.
    /// </summary>
    /// <remarks>This attribut replaces the ReadonlyAttribute and ExcludeFromMetadataAttribute</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsdlIntegerPropertyAttribute : CsdlPropertyAttribute
    {
        public long Min { get; set; }
        public long Max { get; set; }
    }
}