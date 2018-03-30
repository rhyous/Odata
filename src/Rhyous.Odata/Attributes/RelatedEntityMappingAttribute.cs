using System;
using System.Runtime.InteropServices;

namespace Rhyous.Odata
{
    /// <summary>
    /// This is used at the class level to mark a class as having a related entity where the key is on the foreign entity.
    /// </summary>
    /// <remarks>The RelatedEntity must be configured on the mapping entity.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedEntityMappingAttribute : RelatedEntityBaseAttribute
    {
        public const string DefaultKey = "Id";
        public const string DefaultForeignKeyProperty = "{0}Id";

        public RelatedEntityMappingAttribute(string relatedEntity, string mappingEntity, string entity, [Optional] string foreignKeyProperty, [Optional] string KeyProperty, [Optional] Type KeyPropertyType, [Optional] bool autoExpand, [Optional] string entityKeyProperty, [Optional] Type entityKeyPropertyType, [Optional] bool getAll)
        {
            RelatedEntity = relatedEntity;
            MappingEntity = mappingEntity;
            Entity = entity;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? string.Format(DefaultForeignKeyProperty, entity) : foreignKeyProperty;
            KeyProperty = string.IsNullOrWhiteSpace(KeyProperty) ? DefaultKey : KeyProperty;
            KeyPropertyType = KeyPropertyType ?? typeof(int);
            AutoExpand = autoExpand;
            EntityKeyProperty = string.IsNullOrWhiteSpace(entityKeyProperty) ? DefaultKey : entityKeyProperty;
            EntityKeyPropertyType = entityKeyPropertyType ?? typeof(int);
            GetAll = getAll;
        }
        
        /// <summary>
        /// This entity id of the current entity. This is usually Id.
        /// </summary>
        public string EntityKeyProperty { get; set; }
        
        /// <summary>
        /// This type of the entity id of the current entity.
        /// </summary>
        public Type EntityKeyPropertyType { get; set; }
        
        /// <summary>
        /// This mapping entity that links the Entity to the RelatedEntity.
        /// </summary>
        public string MappingEntity { get; set; }

        /// <summary>
        /// The property in the MappingEntity that references this entity.
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


        /// <summary>
        /// An alias for the Mapping Related Entity. This is useful when an entity is related to the same Entity in two ways.
        /// </summary>
        public string MappingEntityAlias { get; set; }
    }
}