﻿namespace Rhyous.Odata
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
        public string EntityIdProperty { get; set; } = "Id";

        /// <summary>
        /// For OneToOne, this is the name of the property in the current entity that has the Id of the related entity.
        /// For ManyToOne or OneToOneForeign, this is the name of the property in the related entity that has the Id of the current entity.
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
                    EntityToRelatedEntityProperty = value + "Id";
            }
        } private string _RelatedEntity;

        /// <summary>
        /// The name of the Id property in the related entity. This should be just Id. So Id is the default.
        /// </summary>
        public string RelatedEntityIdProperty { get; set; } = "Id";

        /// <summary>
        /// Used if the RelatedEntity.Type is ManyToMany.
        /// </summary>
        public string MappingEntity { get; set; }
    }
}