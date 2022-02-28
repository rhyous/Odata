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
    public class OdataObjectCollection : OdataObjectCollectionBase, IList<OdataObject>
    {
        /// <summary>
        /// A list of Entities
        /// </summary>
        [DataMember]
        [JsonProperty]
        internal ParentedList<OdataObject> Entities
        {
            get { return _Entities ?? (_Entities = new ParentedList<OdataObject>(this)); }
            set { _Entities = value; }
        } private ParentedList<OdataObject> _Entities;
        
        #region Implicit Operator
        /// <summary>
        /// This will convert an ODataObjectCollection to a RelatedEntityCollection, but it won't know
        /// which Entity or EntityId it is related to and so these properties should be set.
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator RelatedEntityCollection(OdataObjectCollection c)
        {
            if (c == null)
                return null;
            var rec = new RelatedEntityCollection()
            {                
                RelatedEntity = c.Entity,
            };
            rec.RelatedEntities.AddRange(c.Entities.Select(e => (RelatedEntity)e));
            return rec;
        }
        #endregion

        #region IList implementation
        public void Add(OdataObject item)
        {
            Entities.Add(item);
            item.Parent = this;
        }

        public void AddRange(IEnumerable<OdataObject> items)
        {
            Entities.AddRange(items);
            foreach (var item in items)
            {
                item.Parent = this;
            }
        }

        public int IndexOf(OdataObject item) => Entities.IndexOf(item);

        public void Insert(int index, OdataObject item)
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

        public bool Contains(OdataObject item) => Entities.Contains(item);

        public void CopyTo(OdataObject[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(OdataObject item) => Entities.Remove(item);

        public IEnumerator<OdataObject> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [DataMember]
        [JsonProperty]
        public override int Count { get { return Entities.Count; } protected set { } }

        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsReadOnly => false;

        public OdataObject this[int index]
        {
            get { return Entities[index]; }
            set { Entities[index] = value; }
        }
        #endregion
    }
}