using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Rhyous.Odata
{
    public class RelatedEntityManyToMany : RelatedEntity
    {
        /// <summary>
        /// If set to true, this related entity is added once to the top parent.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsSingleton { get; set; }

        /// <summary>
        /// If set to true, this related entity includes all instances at the time of the call.
        /// This is intended to be used for small mapping tables.
        /// </summary>
        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsAll { get; set; }

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
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("RelatedIdProperty", string.Format(Constants.StringNullException, "RelatedIdProperty"));
                if (_RelatedIdProperty == value)
                    return;
                _RelatedIdProperty = value;
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
            RelatedId = jObj.GetValue(RelatedIdProperty)?.ToString() ?? RelatedId;
        }
    }
}