using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// An attribute to help configure csdl.
    /// </summary>
    /// <remarks>This attribut replaces the ReadonlyAttribute and ExcludeFromMetadataAttribute</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsdlFilePropertyAttribute : CsdlPropertyAttribute
    {
        private string[] All = new string[] { "*" };
        public CsdlFilePropertyAttribute(string fileType, params string[] allowedFileExtensions)
        {
            if (string.IsNullOrWhiteSpace(fileType)) { throw new ArgumentException($"'{nameof(fileType)}' cannot be null or whitespace.", nameof(fileType)); }

            FileType = fileType;
            AllowedFileExtensions = allowedFileExtensions;
            if (AllowedFileExtensions == null || !AllowedFileExtensions.Any())
            {
                if (Csdl.AllowedFileExtensions.Instance.TryGetValue(fileType, out List<string> allowedFileExtensionsList))
                    AllowedFileExtensions = allowedFileExtensionsList.ToArray();
                else
                    AllowedFileExtensions = All;
            }
        }

        /// <summary>A well-known file type:
        /// File = Any file type.
        /// Image = Any image file type.</summary>
        public string FileType { get; }
        public string[] AllowedFileExtensions { get; }
    }
}