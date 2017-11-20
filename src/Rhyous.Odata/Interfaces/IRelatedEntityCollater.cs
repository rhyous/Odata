using System.Collections.Generic;

namespace Rhyous.Odata
{
    public interface IRelatedEntityCollater<T, TId>
    {
        void Collate(IEnumerable<OdataObject<T, TId>> odataEntities, IEnumerable<RelatedEntityCollection> collections);

    }
}
