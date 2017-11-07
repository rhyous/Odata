using System.Collections.Generic;

namespace Rhyous.Odata
{
    public interface IRelatedEntitySorter<T, TId>
    {
        void Collate(IEnumerable<OdataObject<T, TId>> odataEntities, IEnumerable<RelatedEntityCollection> collections);
        List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details);        

    }
}