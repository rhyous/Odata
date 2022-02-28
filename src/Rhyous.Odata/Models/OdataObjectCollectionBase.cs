using Newtonsoft.Json;
using Rhyous.Collections;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    [JsonObject]
    [DataContract]
    public abstract class OdataObjectCollectionBase : IClearable, ICountable
    {
        public OdataObjectCollectionBase() { }
        public OdataObjectCollectionBase(string entity) => Entity = entity;

        /// <summary>
        /// The name of the Entity type returned
        /// </summary>
        [DataMember]
        [JsonProperty]
        public virtual string Entity { get; protected internal set; }

        [DataMember]
        [JsonProperty]
        public abstract int Count { get; protected set; }

        /// <summary>
        /// This provides a place to put a total count when $top is called.
        /// If $top=100 is called, Count will be 100, but for paging, we need
        /// to know that it is 100 of N, where N is the total count had $top
        /// not been called.
        /// </summary>
        [DataMember]
        [JsonProperty]
        public virtual int TotalCount
        {
            get
            { return _TotalCount < Count ? Count : _TotalCount; }
            set { _TotalCount = value; }
        } private int _TotalCount;

        /// <summary>Clears the elements.</summary>
        public virtual void Clear() { RelatedEntityCollection.Clear(); }


        /// <summary>
        /// A list of RelatedEntityCollections. This provides a place for common entities to be included,
        /// so they don't have to be repeated. This will result in much smaller json/xml serialization.
        /// Imagine a list of users that each are a member of a UserGroup. 
        /// </summary>
        [DataMember]
        [JsonProperty]
        public virtual ParentedList<RelatedEntityCollection> RelatedEntityCollection
        {
            get { return _RelatedEntityCollection ?? (_RelatedEntityCollection = new ParentedList<RelatedEntityCollection>()); }
            set { _RelatedEntityCollection = value; }
        } private ParentedList<RelatedEntityCollection> _RelatedEntityCollection;
    }
}
