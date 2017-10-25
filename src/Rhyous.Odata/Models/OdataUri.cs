using System;

namespace Rhyous.Odata
{
    /// <summary>
    /// This object is used to map an Entity's property to a uri.
    /// </summary>
    public class OdataUri
    {
        /// <summary>
        /// The name of a property of an entity.
        /// </summary>
        public virtual string PropertyName { get; set; }

        /// <summary>
        /// The web service uri to manage the property of an entity. 
        /// </summary>
        public virtual Uri Uri { get; set; }
    }
}
