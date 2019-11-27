using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rhyous.Odata.Csdl
{
    public static class PropertyInfoExtensions
    {
        public static bool ExcludeFromMetadata(this PropertyInfo propInfo) 
        {
            return propInfo.GetCustomAttributes(true).Any(a => a is ExcludeFromMetadataAttribute || a is JsonIgnoreAttribute || a is IgnoreDataMemberAttribute);
        }
    }
}
