using Newtonsoft.Json;
using Rhyous.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [JsonObject]
    [DataContract]
    public class OdataObjectCollection<TEntity, TId> : OdataObjectCollectionBase,
                                                       IList<OdataObject<TEntity, TId>>
    {
        public OdataObjectCollection() : base(typeof(TEntity).Name) { }

        /// <summary>
        /// A list of Entities
        /// </summary>
        [DataMember]
        [JsonProperty]
        internal ParentedList<OdataObject<TEntity, TId>> Entities
        {
            get { return _Entities ?? (_Entities = new ParentedList<OdataObject<TEntity, TId>>()); }
            set { _Entities = value; }
        } private ParentedList<OdataObject<TEntity, TId>> _Entities;

        #region Implicit Operator
        /// <summary>
        /// This will convert an ODataObjectCollection to a RelatedEntityCollection, but it won't know
        /// which Entity or EntityId it is related to and so these properties should be set.
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator RelatedEntityCollection(OdataObjectCollection<TEntity, TId> c)
        {
            if (c == null)
                return null;
            var rec = new RelatedEntityCollection()
            {
                RelatedEntity = c.Entity
            };
            rec.RelatedEntities.AddRange(c.Entities.Select(e => (RelatedEntity)e));
            return rec;
        }
        #endregion

        #region IList implementation
        public void Add(OdataObject<TEntity, TId> item)
        {
            Entities.Add(item);
            item.Parent = this;
        }

        public void AddRange(IEnumerable<OdataObject<TEntity, TId>> items)
        {
            Entities.AddRange(items);
            foreach (var item in items)
            {
                item.Parent = this;
            }
        }

        public int IndexOf(OdataObject<TEntity, TId> item) => Entities.IndexOf(item);

        public void Insert(int index, OdataObject<TEntity, TId> item)
        {
            Entities.Insert(index, item);
            item.Parent = this;
        }

        public void RemoveAt(int index) => Entities.RemoveAt(index);

        public override void Clear()
        {
            Entities.Clear();
            base.Clear();
        }

        public bool Contains(OdataObject<TEntity, TId> item) => Entities.Contains(item);

        public void CopyTo(OdataObject<TEntity, TId>[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(OdataObject<TEntity, TId> item) => Entities.Remove(item);

        public IEnumerator<OdataObject<TEntity, TId>> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [DataMember]
        [JsonProperty]
        public override int Count { get { return Entities.Count; } protected set { } }
        
        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsReadOnly => false;

        public OdataObject<TEntity, TId> this[int index]
        {
            get { return Entities[index]; }
            set { Entities[index] = value; }
        }
        #endregion
    }
}