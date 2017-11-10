using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections;

namespace Rhyous.Odata
{
    [JsonObject]
    [DataContract]
    public class RelatedEntityCollection<T, TId> : IList<RelatedEntity<T, TId>>
    {
        /// <summary>
        /// The current entity name, not the related entity name.
        /// </summary>
        [IgnoreDataMember]
        public string Entity { get; set; }

        /// <summary>
        /// The current entity Id or Key, not the related entity Id or Key.
        /// </summary>
        [IgnoreDataMember]
        public string EntityId { get; set; }

        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public TId RelatedEntity { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        internal List<RelatedEntity<T, TId>> Entities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntity<T, TId>>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntity<T, TId>> _RelatedEntities;

        #region Implicit Operator
        public static implicit operator RelatedEntityCollection(RelatedEntityCollection<T, TId> c)
        {
            var rec = new RelatedEntityCollection()
            {
                Entity = c.Entity,
                EntityId = c.EntityId,
                RelatedEntity = c.RelatedEntity.ToString(),
            };
            rec.Entities.AddRange(c.Entities.Select(e => e as RelatedEntity));
            return rec;
        }
        #endregion

        #region IList implementation
        public int Count => Entities.Count;

        public bool IsReadOnly => ((IList<RelatedEntity<T, TId>>)Entities).IsReadOnly;

        public RelatedEntity<T, TId> this[int index] { get => Entities[index]; set => Entities[index] = value; }

        public int IndexOf(RelatedEntity<T, TId> item) => Entities.IndexOf(item);

        public void Insert(int index, RelatedEntity<T, TId> item)
        {
            Entities.Insert(index, item);
        }

        public void RemoveAt(int index) => Entities.RemoveAt(index);

        public void Add(RelatedEntity<T, TId> item)
        {
            Entities.Add(item);
        }

        public void Clear() => Entities.Clear();

        public bool Contains(RelatedEntity<T, TId> item) => Entities.Contains(item);

        public void CopyTo(RelatedEntity<T, TId>[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(RelatedEntity<T, TId> item) => Entities.Remove(item);

        public IEnumerator<RelatedEntity<T, TId>> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
        #endregion
    }
}