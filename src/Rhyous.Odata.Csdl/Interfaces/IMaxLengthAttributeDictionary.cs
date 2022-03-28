using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public interface IMaxLengthAttributeDictionary
    {
        ulong GetMaxLength(PropertyInfo pi);
    }
}