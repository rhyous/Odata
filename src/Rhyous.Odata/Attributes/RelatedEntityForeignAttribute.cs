using System;
using System.Runtime.InteropServices;

namespace Rhyous.Odata
{
    /// <summary>
    /// This is used at the class level to mark a class as having a related entity where the key is on the foreign entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedEntityForeignAttribute : RelatedEntityBaseAttribute
    {
        public const string DefaultKey = "Id";
        public const string DefaultForeignKeyProperty = "{0}Id";

        public RelatedEntityForeignAttribute(string relatedEntity, string entity, [Optional] string foreignKeyProperty, [Optional] string entityProperty, [Optional] Type entityPropertyType, [Optional] bool autoExpand, [Optional] string entityKeyProperty, [Optional] Type entityKeyPropertyType, [Optional] bool getAll)
        {
            RelatedEntity = relatedEntity;
            Entity = entity;
            EntityProperty = entityProperty;
            EntityPropertyType = entityPropertyType;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? string.Format(DefaultForeignKeyProperty, entity) : foreignKeyProperty;
            EntityKeyProperty = string.IsNullOrWhiteSpace(entityKeyProperty) ? DefaultKey : entityKeyProperty;
            EntityKeyPropertyType = entityKeyPropertyType ?? typeof(int);
            AutoExpand = autoExpand;
            GetAll = getAll;
        }

        /// <summary>
        /// This name of the property in the current entity that the foreign entity uses to map to.
        /// Set this only if the foreign key does not reference the EntityKeyProperty.
        /// If not set, EntityKeyProperty should be used.
        /// </summary>
        public string EntityProperty { get; set; }

        /// <summary>
        /// This type of the EntityProperty of the current entity.
        /// </summary>
        public Type EntityPropertyType { get; set; }

        /// <summary>
        /// This entity id of the current entity. This is usually Id.
        /// </summary>
        public string EntityKeyProperty { get; set; }

        /// <summary>
        /// This type of the entity id of the current entity.
        /// </summary>
        public Type EntityKeyPropertyType { get; set; }

        /// <summary>
        /// The property in the RelatedEntity that references this entity's Id or AlternateKey property.
        /// </summary>
        public string ForeignKeyProperty { get; set; }

        /// <summary>
        /// The name of the property identifier on the related entity.
        /// This property is usually "Id", but could be an AlternateKey property.
        /// </summary>
        public string ForeignIdProperty { get; set; }

        /// <summary>
        /// The type of the property identifier on the related entity.
        /// This type is usually the type of the key property "Id", but could be an AlternateKey property's type.
        /// </summary>
        public Type ForeignIdPropertyType { get; set; }
    }
}