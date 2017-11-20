using System.Collections.Generic;

namespace Rhyous.Odata
{
    public interface IRelatedEntitySorter<T>
    {
        List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details);        
    }
}