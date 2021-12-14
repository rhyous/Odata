using Newtonsoft.Json.Linq;
using System;

namespace Rhyous.Odata
{
    public static class JRawExtensions
    {
        public static string GetValueAsString(this JRaw jRaw, string property)
        {
            return jRaw.GetValue(property)?.ToString();
        }

        public static JToken GetValue(this JRaw jRaw, string property)
        {
            if (jRaw == null)
                throw new ArgumentNullException("jRaw", string.Format(Constants.ObjectNullException, "jRaw"));
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentNullException("property", string.Format(Constants.StringNullException, "property"));
            var json = jRaw.ToString();
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException("jRaw", string.Format(Constants.StringNullException, "jRaw"));
            var jObj = JObject.Parse(json);
            return jObj.GetValue(property);
        }
    }
}
