using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    [JsonObject]
    [DataContract]
    public class RelatedEntityCollection : IList<RelatedEntity>
    {
        /// <summary>
        /// The current entity name, not the related entity name.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public string Entity { get; set; }

        /// <summary>
        /// The current entity Id or Key, not the related entity Id or Key.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public string EntityId { get; set; }

        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public string RelatedEntity { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        internal List<RelatedEntity> Entities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntity>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntity> _RelatedEntities;

        #region IList implementation
        public int Count => Entities.Count;

        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsReadOnly => ((IList<RelatedEntity>)Entities).IsReadOnly;

        public RelatedEntity this[int index] { get => Entities[index]; set => Entities[index] = value; }

        public int IndexOf(RelatedEntity item) => Entities.IndexOf(item);

        public void Insert(int index, RelatedEntity item)
        {
            Entities.Insert(index, item);
        }

        public void RemoveAt(int index) => Entities.RemoveAt(index);

        public void Add(RelatedEntity item) => Entities.Add(item);

        public void Clear() => Entities.Clear();

        public bool Contains(RelatedEntity item) => Entities.Contains(item);

        public void CopyTo(RelatedEntity[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(RelatedEntity item) => Entities.Remove(item);

        public IEnumerator<RelatedEntity> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Entities.GetEnumerator();
        #endregion
    }
}