using Newtonsoft.Json;
using Rhyous.Collections;
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
    public class OdataObject<TEntity, TId> : IRelatedEntityCollection
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
        public string IdProperty
        {
            get { return _IdProperty ?? (_IdProperty = "Id"); }
            set
            {
                if (_IdProperty == value)
                    return;
                _IdProperty = value;
                SetId(Object);
            }
        } private string _IdProperty;

        /// <summary>
        /// The entity instance.
        /// </summary>
        [DataMember(Order = 2)]
        public virtual TEntity Object
        {
            get { return _Object; }
            set
            {
                if (Equals(value, _Object))
                    return;
                _Object = value;
                SetId(value);
            }
        } private TEntity _Object;

        /// <summary>
        /// Any related entity for the entity.
        /// </summary>
        [DataMember(Order = 4)]
        public virtual ParentedList<RelatedEntityCollection> RelatedEntityCollection
        {
            get { return _RelatedEntityCollection ?? (_RelatedEntityCollection = new ParentedList<RelatedEntityCollection>(this)); }
            set { _RelatedEntityCollection = value; }
        } private ParentedList<RelatedEntityCollection> _RelatedEntityCollection;

        [JsonIgnore]
        [IgnoreDataMember]
        public object Parent { get; set; }

        /// <summary>
        /// A list of uris that can manage each entity property individually.
        /// </summary>
        [DataMember(Order = 5)]
        public virtual List<OdataUri> PropertyUris { get; set; }

        internal protected virtual void SetId(TEntity value)
        {
            if (value == null)
                return;
            var idProp = value.GetType().GetProperty(IdProperty);
            if (idProp != null)
                Id = (TId)idProp.GetValue(value);
        }

        #region Implicit Operator
        public static implicit operator RelatedEntity(OdataObject<TEntity, TId> o)
        {
            return o.AsRelatedEntity();
        }
        #endregion
    }
}