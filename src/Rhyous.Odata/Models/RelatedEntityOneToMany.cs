using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    public class RelatedEntityOneToMany : RelatedEntity
    {
        public RelatedEntityOneToMany(string relatedIdProperty) { RelatedIdProperty = relatedIdProperty; }
        public RelatedEntityOneToMany(string relatedIdProperty, OdataObject<JRaw,string> obj) : this(relatedIdProperty)
        {
            obj.AsRelatedEntity(this);        
        }

        public RelatedEntityOneToMany(string relatedIdProperty, OdataObject obj) : this(relatedIdProperty)
        {
            obj.AsRelatedEntity(this);
        }

        /// <inheritdoc />
        public override JRaw Object
        {
            get { return base.Object; }
            set
            {
                if (base.Object == value)
                    return;
                base.Object = value;
                SetRelatedId(value);
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public string RelatedIdProperty
        {
            get { return _RelatedIdProperty; }
            set
            {
                if (_RelatedIdProperty == value)
                    return;
                _RelatedIdProperty = value;
                if (string.IsNullOrWhiteSpace(_RelatedIdProperty))
                    throw new ArgumentException("RelatedIdProperty cannot be null, empty, or whitespace.", "RelatedIdProperty");
                SetRelatedId(Object);
            }
        } private string _RelatedIdProperty;

        [JsonIgnore]
        [IgnoreDataMember]
        public string RelatedId { get; set; }

        internal protected void SetRelatedId(JRaw value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return;
            var jObj = JObject.Parse(value.ToString());
            RelatedId = jObj.GetId(RelatedIdProperty) ?? RelatedId;
        }
    }
}