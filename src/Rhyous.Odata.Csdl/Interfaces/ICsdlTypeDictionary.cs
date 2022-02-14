using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICsdlTypeDictionary : IDictionary<string, string>
    {
        void AddBidirectionally(string a, string b);
        void Init();
    }
}