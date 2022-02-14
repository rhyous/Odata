using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomPropertyDataAppender
    {
        void Append(IDictionary<string, object> dictionary, string entity, string property);
    }
}