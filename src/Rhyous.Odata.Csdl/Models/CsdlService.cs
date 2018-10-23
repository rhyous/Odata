using Newtonsoft.Json;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public class CsdlService
    {
        [JsonExtensionData]
        public Dictionary<string, object> Entities
        {
            get { return _Entities ?? (_Entities = new Dictionary<string, object>()); }
            set { _Entities = value; }
        } private Dictionary<string, object> _Entities;
    }
}
