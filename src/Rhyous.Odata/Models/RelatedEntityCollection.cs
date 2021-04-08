using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// The related entity name, not the current entity name.
        /// </summary>
        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public string RelatedEntity { get; set; }

        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        internal ParentedList<RelatedEntity> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new ParentedList<RelatedEntity>(this)); }
            set { _RelatedEntities = value; }
        } private ParentedList<RelatedEntity> _RelatedEntities;

        [JsonIgnore]
        [IgnoreDataMember]
        public object Parent { get; set; }

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

        #region Implicit Operator
        /// <summary>
        /// This will convert an ODataObjectCollection to a RelatedEntityCollection, but it won't know
        /// which Entity or EntityId it is related to and so these properties shoudl be set.
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator OdataObjectCollection(RelatedEntityCollection c)
        {
            if (c == null)
                return null;
            var rec = new OdataObjectCollection()
            {
                Entity = c.RelatedEntity
            };
            rec.Entities.AddRange(c.RelatedEntities.Select(e => (OdataObject)e));
            return rec;
        }
        #endregion
    }
}