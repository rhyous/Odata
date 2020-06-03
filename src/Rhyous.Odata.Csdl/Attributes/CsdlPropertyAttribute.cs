using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// An attribute to help configure csdl.
    /// </summary>
    /// <remarks>This attribut replaces the ReadonlyAttribute and ExcludeFromMetadataAttribute</remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsdlPropertyAttribute : Attribute
    {


        /// <summary>
        /// Whether UI input to this property is required or not. This adds @UI.Required to the metadata
        /// if it is not the default. The default is !Nullable.
        /// UI implementors should give this precedence over Nullable.
        /// </summary>
        /// <remarks>If a value is nullable, it may also not be required.</remarks>
        public bool Required
        {
            get { return _Required ?? true; }
            set { _Required = value; RequiredSet = true; }
        } private bool? _Required;


        /// <summary>
        /// A flag indicating whether the Required value has been set or not.
        /// Default: False.
        /// </summary>
        public bool RequiredSet { get; internal set; }

        /// <summary>
        /// This indicates that a field is nullable to the UI. However, it doesn't indicate the
        /// field is nullable. This field is used so primitive property, such as "int", can be 
        /// marked as nullable without changing the type to a nullable type, such as "int?". This
        /// means the primitive default value will be used or the server side must provide a value.
        /// This field is used in conjunction with IsRequired.
        /// </summary>
        /// <remarks>If you want the actual data to be nullable, make the type nullable.</remarks>
        public bool Nullable
        {
            get { return _Nullable ?? true; }
            set { _Nullable = value; NullableSet = true; }
        } private bool? _Nullable;

        /// <summary>
        /// A flag indicating wether the Nullable value has been set or not.
        /// Default: False.
        /// </summary>
        public bool NullableSet { get; internal set; }

        /// <summary>
        /// This indicates the field is readonly and cannot be edited.
        /// Default: False.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// This excludes a property from Metadata.
        /// Default: False.
        /// </summary>
        public bool ExcludeFromMetadata { get; set; }

        /// <summary>This puts the expected default value into the csdl metadata.</summary>
        public object DefaultValue { get; set; }

        /// <summary>This allows for a custom metadata type.</summary>
        public string CsdlType { get; set; }

        /// <summary>This sets the @UI.MinLength value in csdl metadata..</summary>
        public ulong MinLength { get; set; }

        /// <summary>
        /// This sets both the the standard Odata $Maxlength and the custom @UI.MaxLength value in csdl metadata.
        /// </summary>
        public ulong MaxLength { get; set; }

        /// <summary>This sets the @UI.Hint value in csdl metadata.</summary>
        /// <remarks>UI implementors can use this for hint popups.</remarks>
        public string UIHint { get; set; }

        /// <summary>This sets the @Precision value in $Metadata.</summary>
        public uint Precision { get; set; }
    }
}
