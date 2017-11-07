using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [JsonObject]
    [DataContract]
    public class OdataObjectCollection<TEntity, TId> : IList<OdataObject<TEntity, TId>>
    {
        /// <summary>
        /// A list of Entities
        /// </summary>
        [JsonProperty]
        internal List<OdataObject<TEntity, TId>> Entities
        {
            get { return _Entities ?? (_Entities = new List<OdataObject<TEntity, TId>>()); }
            set { _Entities = value; }
        } private List<OdataObject<TEntity, TId>> _Entities;

        /// <summary>
        /// A list of RelatedEntityCollections. This provides a place for common entities to be included,
        /// so they don't have to be repeated. This will result in much smaller json/xml serialization.
        /// Imagine a list of users that each are a member of a UserGroup. 
        /// </summary>
        [JsonProperty]
        public virtual List<RelatedEntityCollection> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntityCollection>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntityCollection> _RelatedEntities;

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

        public int IndexOf(OdataObject<TEntity, TId> item)
        {
            return Entities.IndexOf(item);
        }

        public void Insert(int index, OdataObject<TEntity, TId> item)
        {
            Entities.Insert(index, item);
            item.Parent = this;
        }

        public void RemoveAt(int index)
        {
            Entities.RemoveAt(index);
        }

        public void Clear()
        {
            Entities.Clear();
        }

        public bool Contains(OdataObject<TEntity, TId> item)
        {
            return Entities.Contains(item);
        }

        public void CopyTo(OdataObject<TEntity, TId>[] array, int arrayIndex)
        {
            Entities.CopyTo(array, arrayIndex);
        }

        public bool Remove(OdataObject<TEntity, TId> item)
        {
            return Entities.Remove(item);
        }

        public IEnumerator<OdataObject<TEntity, TId>> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => Entities.Count;

        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsReadOnly => false;

        public OdataObject<TEntity, TId> this[int index]
        {
            get { return Entities[index]; }
            set { Entities[index] = value; }
        }
    }
}