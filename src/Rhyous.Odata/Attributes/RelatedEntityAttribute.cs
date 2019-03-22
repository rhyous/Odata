using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rhyous.Odata
{
    /// <summary>
    /// An attribute that specifies the related entity for a foreign key property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RelatedEntityAttribute : RelatedEntityBaseAttribute
    {
        public const string DefaultForeignKey = "Id";

        public RelatedEntityAttribute(string relatedEntity, [Optional] string foreignKeyProperty, [Optional] Type foreignKeyType, [Optional] bool autoExpand, [CallerMemberName]string property = null)
        {
            RelatedEntity = relatedEntity;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? DefaultForeignKey : foreignKeyProperty;
            foreignKeyType = foreignKeyType ?? typeof(int);
            AutoExpand = autoExpand;
            Property = property;
        }
        
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
        
        /// <summary>
        /// The name of the Property this attribute decorates.
        /// [CallerMemberName] should take care of setting this.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// If this is true, the property that references the related entity can be null.
        /// is passed to the web service.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// If this is true is will enforce that the related entity exists and throw an exception if not. 
        /// It defaults to TRUE.
        /// </summary>
        public bool RelatedEntityMustExist { get; set; } = true;
    }
}