using System;
using System.Runtime.InteropServices;

namespace Rhyous.Odata
{
    /// <summary>
    /// This is used at the class level to mark a class as having a related entity where the key is on the foreign entity.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedEntityForeignAttribute : Attribute, IRelatedEntity
    {
        public const string DefaultKey = "Id";
        public const string DefaultForeignKeyProperty = "{0}Id";

        public RelatedEntityForeignAttribute(string relatedEntity, string entity, [Optional] string foreignKeyProperty, [Optional] string KeyProperty, [Optional] Type KeyPropertyType, [Optional] bool autoExpand, [Optional] string entityKeyProperty, [Optional] Type entityKeyPropertyType, [Optional] bool getAll)
        {
            RelatedEntity = relatedEntity;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? string.Format(DefaultForeignKeyProperty, entity) : foreignKeyProperty;
            KeyProperty = string.IsNullOrWhiteSpace(KeyProperty) ? DefaultKey : KeyProperty;
            KeyPropertyType = KeyPropertyType ?? typeof(int);
            AutoExpand = autoExpand;
            Entity = entity;
            EntityKeyProperty = string.IsNullOrWhiteSpace(entityKeyProperty) ? DefaultKey : entityKeyProperty;
            EntityKeyPropertyType = entityKeyPropertyType ?? typeof(int);
            GetAll = getAll;
        }

        /// <summary>
        /// This entity the attribute is applied to.
        /// </summary>
        /// <remarks>[CallerMemberName] doesn't work at the class level, so this must be specified.</remarks>
        public string Entity { get; set; }

        /// <summary>
        /// This entity id of the current entity. This is usually Id.
        /// </summary>
        public string EntityKeyProperty { get; set; }


        /// <summary>
        /// This type of the entity id of the current entity.
        /// </summary>
        public Type EntityKeyPropertyType { get; set; }

        /// <summary>
        /// The name of the related entity.
        /// </summary>
        public string RelatedEntity { get; set; }

        /// <summary>
        /// The property in the RelatedEntity that references this entity.
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
        
        /// <inheritdoc />
        public bool AutoExpand { get; set; }

        /// <inheritdoc />
        public bool GetAll { get; set; }
    }
}