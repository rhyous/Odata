using Rhyous.Collections;
using Rhyous.StringLibrary;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntitySorter<T, TId> : IRelatedEntitySorter<T>, IRelatedEntityCollater<T, TId>
    {
        public void Collate(IEnumerable<OdataObject<T, TId>> odataEntities, IEnumerable<RelatedEntityCollection> collections)
        {
            if (odataEntities == null || !odataEntities.Any() || collections == null || !collections.Any())
                return;
            var dictionary = new Dictionary<TId, List<ParentedList<RelatedEntityCollection>>>();
            foreach (var odataEntity in odataEntities)
            {
                if (dictionary.TryGetValue(odataEntity.Id, out List<ParentedList<RelatedEntityCollection>> list))
                {
                    list.Add(odataEntity.RelatedEntityCollection);
                    continue;
                }
                dictionary.Add(odataEntity.Id, new List<ParentedList<RelatedEntityCollection>> { odataEntity.RelatedEntityCollection });
            }
            foreach (RelatedEntityCollection collection in collections)
            {
                var id = (TId)collection.EntityId.ToType(typeof(TId));
                foreach (var collectionList in dictionary[id])
                {
                    collectionList.Add(collection);
                }
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