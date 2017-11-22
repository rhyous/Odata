﻿using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntitySorter<T, TId> : IRelatedEntitySorter<T>, IRelatedEntityCollater<T, TId>
    {
        public void Collate(IEnumerable<OdataObject<T, TId>> odataEntities, IEnumerable<RelatedEntityCollection> collections)
        {
            if (odataEntities == null || !odataEntities.Any() || collections == null || !collections.Any())
                return;
            foreach (var odataEntity in odataEntities)
            {
                var matchingCollections = collections.Where(c => c.EntityId == odataEntity.Id.ToString()).ToList();
                odataEntity.RelatedEntityCollection.AddRange(matchingCollections);
            }
        }
        public List<RelatedEntityCollection> Sort(IEnumerable<T> entities, IEnumerable<RelatedEntity> relatedEntities, SortDetails details)
        {
            return SortMethodDictionary[details.RelatedEntityType](entities, relatedEntities, details);
        }

        public SortMethodDictionary<T> SortMethodDictionary
        {
            get { return _SortMethodDictionary ?? (_SortMethodDictionary = new SortMethodDictionary<T>()); }
            set { _SortMethodDictionary = value; }
        } private SortMethodDictionary<T> _SortMethodDictionary;
    }
}