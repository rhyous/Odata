using Rhyous.Collections;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public interface ICustomPropertyDataAppender
    {
        void Append(IConcurrentDictionary<string, object> dictionary, string entity, string property);
    }
}