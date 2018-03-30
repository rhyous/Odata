using System;

namespace Rhyous.Odata
{
    public static class SortDetailsExtensions
    {
        public static RelatedEntityCollection ToRelatedEntityCollection(this SortDetails details, string id)
        {
            if (details == null)
                throw new ArgumentNullException("details", string.Format(Constants.ObjectNullException, "details"));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("id",string.Format(Constants.StringNullException, "id"));
            return new RelatedEntityCollection
            {
                Entity = details.EntityName,
                EntityId = id,
                RelatedEntity = details.RelatedEntity,
            };
        }
    }
}
