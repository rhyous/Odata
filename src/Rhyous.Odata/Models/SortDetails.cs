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
        public string EntityIdProperty
        {
            get { return _entityIdProperty ?? (_entityIdProperty = Constants.DefaultIdProperty); }
            set { _entityIdProperty = value; }
        } private string _entityIdProperty;

        /// <summary>
        /// This name of the property in the current entity that the foreign entity uses to map to.
        /// Set this only if the foreign key does not reference the EntityIdProperty.
        /// If not set, EntityIdProperty should be used.
        /// </summary>
        public string EntityProperty
        {
            get { return _EntityProperty ?? (_EntityProperty = Constants.DefaultIdProperty); }
            set { _EntityProperty = value; }
        } private string _EntityProperty;

        /// <summary>
        /// For RelatedEntity (ManyToOne or OneToOne), this is the name of the property in the current entity that has the Id of the related entity.
        /// For RelatedEntityForeign (OneToMany or OneToOneForeign), this is the name of the property in the related entity that has the Id of the current entity.
        /// For ManyToMany, this is the name of the property in the mapping entity that has the Id of the current entity.
        /// By default EntityToRelatedEntityProperty will return RelatedEntity + "Id".
        /// </summary>
        public string EntityToRelatedEntityProperty
        {
            get { return _EntityToRelatedEntityProperty ?? (RelatedEntity + Constants.DefaultIdProperty); } // Return a default, unless directly set.
            set { _EntityToRelatedEntityProperty = value; }
        } private string _EntityToRelatedEntityProperty;

        /// <summary>
        /// The related entity name. If you set this property, and EntityToRelatedEntityProperty is not set,
        /// </summary>
        public string RelatedEntity { get; set; }

        /// <summary>
        /// The name of the Id property in the related entity. This should be just Id. So Id is the default.
        /// However, if this isn't the Id, but is an AlternateId or even a non-Id field, then that should
        /// still work.
        /// </summary>
        public string RelatedEntityIdProperty
        {
            get { return _RelatedEntityIdProperty ?? (_RelatedEntityIdProperty = Constants.DefaultIdProperty); }
            set { _RelatedEntityIdProperty = value; }
        } private string _RelatedEntityIdProperty;
    }
}