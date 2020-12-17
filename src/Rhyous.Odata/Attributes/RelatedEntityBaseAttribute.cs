using System;

namespace Rhyous.Odata
{
    public abstract class RelatedEntityBaseAttribute : Attribute, IRelatedEntityAttribute
    {
        /// <inheritdoc />
        public virtual string Entity { get; set; }
        /// <inheritdoc />
        public virtual string EntityAlias { get; set; }
        /// <inheritdoc />
        public virtual string RelatedEntity { get; set; }
        /// <inheritdoc />
        public virtual string RelatedEntityAlias { get; set; }
        /// <inheritdoc />
        public virtual bool GetAll { get; set; }
        /// <inheritdoc />
        public virtual bool AutoExpand { get; set; }
        /// <inheritdoc />
        public virtual string Filter { get; set; }
        /// <inheritdoc />
        public virtual string DisplayCondition { get; set; }

    }
}
