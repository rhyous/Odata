using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{ 
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [DataContract]
    public class OdataObject<TEntity, TId>
    {
        /// <summary>
        /// The entity's web service uri.
        /// </summary>
        [DataMember(Order = 0)]
        public virtual Uri Uri { get; set; }

        /// <summary>
        /// The entity instance.
        /// </summary>
        [DataMember(Order = 1)]
        public virtual TId Id { get; set; }

        /// <summary>
        /// If the Id property is not Id, the property name must be specified.
        /// </summary>
        public string IdProperty { get; set; } = "Id";

        /// <summary>
        /// The entity instance.
        /// </summary>
        [DataMember(Order = 2)]
        public virtual TEntity Object
        {
            get { return _Object; }
            set
            {
                _Object = value;
                SetId(value);
            }
        } private TEntity _Object;

        internal protected virtual void SetId(TEntity value)
        {
            var idProp = value.GetType().GetProperty(IdProperty);
            if (idProp != null)
                Id = (TId)idProp.GetValue(value);
        }

        /// <summary>
        /// Any related entity for the entity.
        /// </summary>
        [DataMember(Order = 4)]
        public virtual List<RelatedEntityCollection> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntityCollection>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntityCollection> _RelatedEntities;

        /// <summary>
        /// A list of uris that can manage each entity property individually.
        /// </summary>
        [DataMember(Order = 5)]
        public virtual List<OdataUri> PropertyUris { get; set; }
    }
}