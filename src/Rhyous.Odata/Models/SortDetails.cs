namespace Rhyous.Odata
{
    public class SortDetails
    {
        public SortDetails() { }

        public SortDetails(string entity, string relatedEntity, RelatedEntity.Type relatedEntityType)
        {
            EntityName = entity;
            RelatedEntity = relatedEntity;
            RelatedEntityType = relatedEntityType;
        }

        /// <summary>
        /// The type of the related entity.
        /// </summary>
        public RelatedEntity.Type RelatedEntityType { get; set; }

        /// <summary>
        /// The current entity name.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// The name of the current entity's Id property. This should be just Id. So Id is the default.
        /// </summary>
        public string EntityIdProperty { get; set; } = Constants.DefaultIdProperty;

        /// <summary>
        /// This name of the property in the current entity that the foreign entity uses to map to.
        /// Set this only if the foreign key does not reference the EntityIdProperty.
        /// If not set, EntityIdProperty should be used.
        /// </summary>
        public string EntityProperty { get; set; } = Constants.DefaultIdProperty;

        /// <summary>
        /// For RelatedEntity (ManyToOne or OneToOne), this is the name of the property in the current entity that has the Id of the related entity.
        /// For RelatedEntityForeign (OneToMany or OneToOneForeign), this is the name of the property in the related entity that has the Id of the current entity.
        /// For ManyToMany, this is the name of the property in the mapping entity that has the Id of the current entity.
        /// </summary>
        public string EntityToRelatedEntityProperty { get; set; }

        /// <summary>
        /// The related entity name. If you set this property, and EntityToRelatedEntityProperty is not set,
        /// EntityToRelatedEntityProperty will be set to RelatedEntity + "Id".
        /// </summary>
        public string RelatedEntity
        {
            get { return _RelatedEntity; }
            set
            {
                _RelatedEntity = value;
                if (string.IsNullOrWhiteSpace(EntityToRelatedEntityProperty))
                    EntityToRelatedEntityProperty = value + Constants.DefaultIdProperty;
            }
        } private string _RelatedEntity;

        /// <summary>
        /// The name of the Id property in the related entity. This should be just Id. So Id is the default.
        /// However, if this isn't the Id, but is an AlternateId or even a non-Id field, then that should
        /// still work.
        /// </summary>
        public string RelatedEntityIdProperty { get; set; } = Constants.DefaultIdProperty;
    }
}