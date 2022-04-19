using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// An attribute to help configure csdl for a number with decimal points.
    /// </summary>
    /// <remarks>This attribut replaces the ReadonlyAttribute and ExcludeFromMetadataAttribute</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsdlDecimalPropertyAttribute : CsdlPropertyAttribute
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public ushort Precision { get; set; }
        public short Scale { get; set; }
    }
}