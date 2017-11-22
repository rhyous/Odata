using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    [JsonObject]
    [DataContract]
    public class RelatedEntityCollection : IList<RelatedEntity>, IOdataChild, IOdataParent
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

        /// <summary>
        /// The related entity name, not the current entity name.
        /// </summary>
        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public string RelatedEntity { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        internal List<RelatedEntity> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntity>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntity> _RelatedEntities;

        [JsonIgnore]
        [IgnoreDataMember]
        public IOdataParent Parent { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public List<IOdataChild> Children
        {
            get { return RelatedEntities.ToList<IOdataChild>(); }
            set { }
        }

        #region IList implementation
        public int Count => RelatedEntities.Count;

        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsReadOnly => ((IList<RelatedEntity>)RelatedEntities).IsReadOnly;

        public RelatedEntity this[int index] { get => RelatedEntities[index]; set => RelatedEntities[index] = value; }

        public int IndexOf(RelatedEntity item) => RelatedEntities.IndexOf(item);

        public void Insert(int index, RelatedEntity item)
        {
            RelatedEntities.Insert(index, item);
        }

        public void RemoveAt(int index) => RelatedEntities.RemoveAt(index);

        public void Add(RelatedEntity item) => RelatedEntities.Add(item);

        public void Clear() => RelatedEntities.Clear();

        public bool Contains(RelatedEntity item) => RelatedEntities.Contains(item);

        public void CopyTo(RelatedEntity[] array, int arrayIndex) => RelatedEntities.CopyTo(array, arrayIndex);

        public bool Remove(RelatedEntity item) => RelatedEntities.Remove(item);

        public IEnumerator<RelatedEntity> GetEnumerator() => RelatedEntities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => RelatedEntities.GetEnumerator();
        #endregion
    }
}