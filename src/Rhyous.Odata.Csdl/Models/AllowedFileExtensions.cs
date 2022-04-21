using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class AllowedFileExtensions : ConcurrentDictionary<string, List<string>>
    {
        #region Singleton

        private static readonly Lazy<AllowedFileExtensions> Lazy = new Lazy<AllowedFileExtensions>(() => new AllowedFileExtensions());

        /// <summary>This singleton instance</summary>
        public static AllowedFileExtensions Instance { get { return Lazy.Value; } }

        internal AllowedFileExtensions() : base(StringComparer.OrdinalIgnoreCase)
        {
            TryAdd(FileTypes.Ebook, new List<string> { "azw", "azw1", "azw3", "azw4", "azw6", "epub", "mobi", "pdf" });
            TryAdd(FileTypes.File, new List<string> { "*" });
            TryAdd(FileTypes.Image, new List<string> { "jpg","png","gif","webp","tiff","psd","raw","bmp",
                                                       "heif","indd","common vector image file formats",
                                                       "svg","ai","eps","pdf" });
            TryAdd(FileTypes.Zip, new List<string> { "*" });

        }

        #endregion
    }
}