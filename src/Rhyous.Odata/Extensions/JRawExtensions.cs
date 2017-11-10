using Newtonsoft.Json.Linq;
using System;

namespace Rhyous.Odata
{
    public static class JRawExtensions
    {
        public static string GetIdDynamic(this JObject jObj, string propertySpecifyingIdProperty = "IdProperty")
        {
            if (jObj == null)
                return null;
            var idProp = jObj.GetValue(propertySpecifyingIdProperty)?.ToString();
            return GetId(jObj, idProp);
        }

        public static string GetId(this JObject jObj, string idProp = "Id")
        {
            if (string.IsNullOrWhiteSpace(idProp))
                idProp = "Id";
            var id = jObj.GetValue(idProp)?.ToString();
            if (string.IsNullOrWhiteSpace(id))
                return null;
            return id;
        }
    }
}
