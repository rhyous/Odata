using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    [DataContract]
    public class OdataObject : IRelatedEntityCollection
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
        public virtual string Id { get; set; }

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
        public virtual JRaw Object
        {
            get { return _Object; }
            set
            {
                if (_Object == value)
                    return;
                _Object = value;
                SetId(value);
            }
        } private JRaw _Object;

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

        internal protected void SetId(JRaw value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return;
            var jObj = JObject.Parse(value.ToString());
            Id = jObj.GetIdDynamic() ?? Id;
        }

        #region Implicit Operator
        public static implicit operator RelatedEntity(OdataObject o)
        {
            return o.AsRelatedEntity();
        }
        #endregion
    }
}