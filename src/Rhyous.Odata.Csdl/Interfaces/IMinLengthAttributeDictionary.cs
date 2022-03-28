using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface IMinLengthAttributeDictionary
    {
        ulong GetMinLength(PropertyInfo pi);
    }
}