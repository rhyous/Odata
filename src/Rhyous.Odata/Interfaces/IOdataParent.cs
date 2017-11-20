using System.Collections.Generic;

namespace Rhyous.Odata
{
    public interface IOdataParent
    {
        List<IOdataChild> Children { get; set; }
    }
}
