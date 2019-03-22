using Rhyous.Collections;

namespace Rhyous.Odata
{
    public interface IRelatedEntityCollection
    {
        ParentedList<RelatedEntityCollection> RelatedEntityCollection { get; set; }
    }
}