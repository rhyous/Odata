using System;

namespace Rhyous.Odata
{
    public abstract class RelatedEntityBaseAttribute : Attribute, IRelatedEntityAttribute
    {
        /// <inheritdoc />
        public string Entity { get; set; }
        /// <inheritdoc />
        public string EntityAlias { get; set; }
        /// <inheritdoc />
        public string RelatedEntity { get; set; }
        /// <inheritdoc />
        public string RelatedEntityAlias { get; set; }
        /// <inheritdoc />
        public bool GetAll { get; set; }
        /// <inheritdoc />
        public bool AutoExpand { get; set; }
    }
}
