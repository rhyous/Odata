using Newtonsoft.Json;
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
    public class OdataObjectCollection : IList<OdataObject>, IRelatedEntities
    {
        /// <summary>
        /// The name of the Entity type returned
        /// </summary>
        [DataMember]
        [JsonProperty]
        public string Entity { get; set; }

        /// <summary>
        /// A list of Entities
        /// </summary>
        [DataMember]
        [JsonProperty]
        internal List<OdataObject> Entities
        {
            get { return _Entities ?? (_Entities = new List<OdataObject>()); }
            set { _Entities = value; }
        } private List<OdataObject> _Entities;

        /// <summary>
        /// A list of RelatedEntityCollections. This provides a place for common entities to be included,
        /// so they don't have to be repeated. This will result in much smaller json/xml serialization.
        /// Imagine a list of users that each are a member of a UserGroup. 
        /// </summary>
        [DataMember]
        [JsonProperty]
        public virtual List<RelatedEntityCollection> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntityCollection>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntityCollection> _RelatedEntities;
        
        #region Implicit Operator
        /// <summary>
        /// This will convert an ODataObjectCollection to a RelatedEntityCollection, but it won't know
        /// which Entity or EntityId it is related to and so these properties shoudl be set.
        /// </summary>
        /// <param name="c"></param>
        public static implicit operator RelatedEntityCollection(OdataObjectCollection c)
        {
            var rec = new RelatedEntityCollection()
            {                
                RelatedEntity = c.Entity                
            };
            rec.Entities.AddRange(c.Entities.Select(e => (RelatedEntity)e));
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

        public void Clear() => Entities.Clear();

        public bool Contains(OdataObject item) => Entities.Contains(item);

        public void CopyTo(OdataObject[] array, int arrayIndex) => Entities.CopyTo(array, arrayIndex);

        public bool Remove(OdataObject item) => Entities.Remove(item);

        public IEnumerator<OdataObject> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        [DataMember]
        [JsonProperty]
        public int Count { get { return Entities.Count; } internal set { } }

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