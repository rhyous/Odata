using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    /// <summary>
    /// This extension class makes wrapping objects in Odata types easier.
    /// </summary>
    public static class OdataExtensions
    {
        public const string ObjectUrl = "{0}({1})";

        #region single entity
        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl = null, UriKind uriKind = UriKind.RelativeOrAbsolute, params string[] properties)
        {
            return t.AsOdata<T, TId>(leftPartOfUrl, true, uriKind, properties);
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, Uri uri, params string[] properties)
        {
            var leftPart = uri?.GetLeftPart(UriPartial.Path);
            var uriKind = uri != null && uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
            return t.AsOdata<T, TId>(leftPart, false, uriKind, properties);
        }

        public static OdataObject<T, TId> AsOdata<T, TId>(this T t, string leftPartOfUrl, bool addIdToUrl, UriKind uriKind = UriKind.Relative, params string[] properties)
        {
            var obj = new OdataObject<T, TId> { Object = t, PropertyUris = new List<OdataUri>() };
            obj.SetUri(leftPartOfUrl, uriKind, addIdToUrl);
            // Uncomment below if we decide to publish all Entity property uris
            //if (properties == null || properties.Length == 0)
            //    properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreDataMemberAttribute)))?.Select(p=>p.Name).ToArray();
            obj.AddPropertyUris(properties);
            return obj;
        }
        #endregion

        #region multiple entities
        public static OdataObjectCollection<T, TId> AsOdata<T, TId>(this IEnumerable<T> ts, Uri uri = null, params string[] properties)
        {
            var leftPart = uri?.GetLeftPart(UriPartial.Path);
            var uriKind = uri != null && uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
            var entities = ts.Select(t => t.AsOdata<T, TId>(leftPart, true, uriKind, properties)).ToList();
            var collection = new OdataObjectCollection<T, TId>();
            collection.AddRange(entities);
            return collection;
        }
        #endregion

        #region internal
        private static void SetUri<T, TId>(this OdataObject<T, TId> obj, string leftPartOfUrl, UriKind uriKind, bool addIdToUrl)
        {
            if (!string.IsNullOrWhiteSpace(leftPartOfUrl))
                obj.Uri = addIdToUrl
                        ? new Uri(string.Format(ObjectUrl, leftPartOfUrl, obj.Id), uriKind)
                        : new Uri(leftPartOfUrl, uriKind);
        }

        internal static void AddPropertyUris<T, TId>(this OdataObject<T, TId> obj, string[] properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                    obj.AddProperty(prop);
            }
        }

        internal static void AddProperty<T, TId>(this OdataObject<T, TId> obj, string prop)
        {
            obj.PropertyUris.Add(
                new OdataUri
                {
                    PropertyName = prop,
                    Uri = new Uri("/" + prop, UriKind.Relative)
                }
            );
        }
        #endregion
    }
}
