using System.Collections.Generic;

namespace Rhyous.Odata
{
    public interface IRelatedEntities
    {
        List<RelatedEntityCollection> RelatedEntities { get; set; }
    }
}
