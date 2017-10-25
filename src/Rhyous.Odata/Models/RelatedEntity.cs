using System;
using Newtonsoft.Json.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntity : OdataObject<JRaw, string>
    {
        internal protected override void SetId(JRaw value)
        {
            var jObj = JObject.Parse(value.ToString());
            var idProp = jObj.GetValue("IdProperty")?.ToString();
            if (string.IsNullOrWhiteSpace(idProp))
                idProp = "Id";
            Id = jObj.GetValue(idProp)?.ToString();
            if (string.IsNullOrWhiteSpace(Id))
                throw new InvalidOperationException("The object must have an Id property or the IdProperty must be specified.");
        }
    }
}
