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
        public const string DefaultForeignKey = Constants.DefaultIdProperty;
        public static readonly Type DefaultForeignKeyType = typeof(int);

        public RelatedEntityAttribute(string relatedEntity, [Optional] string foreignKeyProperty, [Optional] Type foreignKeyType, [Optional] bool autoExpand, [CallerMemberName] string property = null)
        {
            RelatedEntity = relatedEntity;
            ForeignKeyProperty = string.IsNullOrWhiteSpace(foreignKeyProperty) ? DefaultForeignKey : foreignKeyProperty;
            ForeignKeyType = foreignKeyType ?? DefaultForeignKeyType;
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
        /// If this is true, the navigation property that references the related entity can be null.
        /// is passed to the web service.
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// If this is true, it will enforce that the related entity must exists and throw an exception if not. 
        /// If this is false, the related entity doesn't have to exist.
        /// It defaults to TRUE.
        /// </summary>
        public bool RelatedEntityMustExist { get; set; } = true;

        /// <summary>
        /// A non-null value that must be used when a RelatedEntity doesn't exist. For example, when the RelatedEntity
        /// is an int, you could make a nonexistant value 0. Whatever the NonExistentValue, it should be a 
        /// value that the repository allows but never uses automatically, such as in autoincrement.
        /// </summary>
        /// <remarks>When using this value, Nullable should be false and RelatedEntityMustExist should be true.
        /// Do not use this to allow NULL, but instead make this attribute nullable and set RelatedEntityMustExist
        /// to false. This value will be the default value in metadata.</remarks>
        public object AllowedNonExistentValue { get; set; }
    }
}