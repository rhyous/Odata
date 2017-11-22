using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rhyous.Odata
{
    /// <summary>
    /// An attribute that specifies the related entity for a foreign key property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedEntityAttribute : Attribute, IRelatedEntity
    {
        public const string DefaultForeignKey = "Id";

        public RelatedEntityAttribute(string entity, [Optional] string foreignKeyProperty, [Optional] Type foreignKeyType, [Optional] bool autoExpand, [CallerMemberName]string property = null)
        {
            RelatedEntity = entity;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? DefaultForeignKey : foreignKeyProperty;
            foreignKeyType = foreignKeyType ?? typeof(int);
            AutoExpand = autoExpand;
            Property = property;
        }

        /// <inheritdoc />
        public string RelatedEntity { get; set; }

        /// <summary>
        /// The name of the property identifier on the related entity.
        /// This key property is usually "Id", but could be an AlternateKey property.
        /// </summary>
        public string ForeignKeyProperty { get; set; }

        /// <summary>
        /// The type of the property identifier on the related entity.
        /// This type is usually the type of the key property "Id", but could be an AlternateKey property's type.
        /// </summary>
        public Type ForeignKeyType { get; set; }
        
        /// <inheritdoc />
        public bool AutoExpand { get; set; }

        /// <inheritdoc />
        public bool GetAll { get; set; }
        
        /// <summary>
        /// The name of the Property this attribute decorates.
        /// </summary>
        public string Property { get; set; }
    }
}