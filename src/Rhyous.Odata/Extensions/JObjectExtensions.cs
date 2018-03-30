using Newtonsoft.Json.Linq;
using System;

namespace Rhyous.Odata
{
    public static class JObjectExtensions
    {
        public static string GetIdDynamic(this JObject jObj, string propertySpecifyingIdProperty = Constants.IdPropertySpeficier)
        {
            if (jObj == null)
                throw new ArgumentNullException("jObj", string.Format(Constants.ObjectNullException, "jObj"));
            var idProp = jObj.GetValue(propertySpecifyingIdProperty)?.ToString();
            if (string.IsNullOrWhiteSpace(idProp))
                idProp = Constants.DefaultIdProperty;
            return jObj.GetValue(idProp)?.ToString();
        }
    }
}
