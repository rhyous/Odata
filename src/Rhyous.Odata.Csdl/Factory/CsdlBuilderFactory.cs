using System;

namespace Rhyous.Odata.Csdl
{
    public class CsdlBuilderFactory : ICsdlBuilderFactory
    {
        #region Singleton

        private static readonly Lazy<CsdlBuilderFactory> Lazy = new Lazy<CsdlBuilderFactory>(() => new CsdlBuilderFactory());

        /// <summary>This singleton instance</summary>
        public static CsdlBuilderFactory Instance { get { return Lazy.Value; } }

        internal CsdlBuilderFactory() { }

        #endregion

        public CsdlBuilderFactory(IEntityBuilder entityBuilder,
                                  IPropertyBuilder propertyBuilder,
                                  IEnumPropertyBuilder enumPropertyBuilder,
                                  ICsdlTypeDictionary csdlTypeDictionary,
                                  IEntityAttributeDictionary entityAttributeDictionary,
                                  IPropertyAttributeDictionary propertyAttributeDictionary,
                                  IPropertyDataAttributeDictionary propertyDataAttributeDictionary,
                                  ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                                  ICustomPropertyFuncs customPropertyFuncs,
                                  ICustomPropertyDataFuncs customPropertyDataFuncs,
                                  IRelatedEntityNavigationPropertyBuilder relatedEntityNavigationPropertyBuilder,
                                  IRelatedEntityForeignNavigationPropertyBuilder relatedEntityForeignNavigationPropertyBuilder,
                                  IRelatedEntityMappingNavigationPropertyBuilder relatedEntityMappingNavigationPropertyBuilder
                                  )
        {
            _EntityBuilder = entityBuilder;
            _PropertyBuilder = propertyBuilder;
            _EnumPropertyBuilder = enumPropertyBuilder;
            CsdlTypeDictionary = csdlTypeDictionary;
            _EntityAttributeDictionary = entityAttributeDictionary;
            _PropertyAttributeDictionary = propertyAttributeDictionary;
            PropertyDataAttributeDictionary = propertyDataAttributeDictionary;
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            CustomPropertyFuncs = customPropertyFuncs;
            CustomPropertyDataFuncs = customPropertyDataFuncs;
            _RelatedEntityNavigationPropertyBuilder = relatedEntityNavigationPropertyBuilder;
            RelatedEntityForeignNavigationPropertyBuilder = relatedEntityForeignNavigationPropertyBuilder;
            RelatedEntityMappingNavigationPropertyBuilder = relatedEntityMappingNavigationPropertyBuilder;
        }

        #region Builders
        public IEntityBuilder EntityBuilder
        {
            get => _EntityBuilder ?? (_EntityBuilder = new EntityBuilder(PropertyBuilder, EnumPropertyBuilder, CustomCsdlBuilder, CustomPropertyAppender));
            internal set => _EntityBuilder = value;
        } private IEntityBuilder _EntityBuilder;

        public IPropertyBuilder PropertyBuilder
        {
            get => _PropertyBuilder ?? (_PropertyBuilder = new PropertyBuilder(CustomCsdlBuilder, CustomPropertyDataAppender, CsdlTypeDictionary));
            internal set => _PropertyBuilder = value;
        } private IPropertyBuilder _PropertyBuilder;

        public IEnumPropertyBuilder EnumPropertyBuilder
        {
            get => _EnumPropertyBuilder ?? (_EnumPropertyBuilder = new EnumPropertyBuilder(CustomCsdlBuilder, CustomPropertyDataAppender, CsdlTypeDictionary));
            internal set => _EnumPropertyBuilder = value;
        } private IEnumPropertyBuilder _EnumPropertyBuilder;

        public ICsdlTypeDictionary CsdlTypeDictionary { get; } = new CsdlTypeDictionary();
        #endregion


        #region Attribute Dictionaries

        public IEntityAttributeDictionary EntityAttributeDictionary
        {
            get => _EntityAttributeDictionary ?? (_EntityAttributeDictionary = new EntityAttributeDictionary(RelatedEntityForeignNavigationPropertyBuilder, 
                                                                                                             RelatedEntityMappingNavigationPropertyBuilder));
            internal set => _EntityAttributeDictionary = value;
        } private IEntityAttributeDictionary _EntityAttributeDictionary;

        public IPropertyAttributeDictionary PropertyAttributeDictionary
        {
            get => _PropertyAttributeDictionary ?? (_PropertyAttributeDictionary = new PropertyAttributeDictionary(RelatedEntityNavigationPropertyBuilder));
            internal set => _PropertyAttributeDictionary = value;
        } private IPropertyAttributeDictionary _PropertyAttributeDictionary;

        public IPropertyDataAttributeDictionary PropertyDataAttributeDictionary { get; } = new PropertyDataAttributeDictionary();

        #endregion

        #region Custom Builders

        public ICustomCsdlFromAttributeAppender CustomCsdlBuilder
        {
            get => _CustomCsdlFromAttributeAppender ?? (_CustomCsdlFromAttributeAppender = new CustomCsdlFromAttributeAppender(EntityAttributeDictionary,
                                                                                                                               PropertyAttributeDictionary,
                                                                                                                               PropertyDataAttributeDictionary));
            set => _CustomCsdlFromAttributeAppender = value;
        } private ICustomCsdlFromAttributeAppender _CustomCsdlFromAttributeAppender;

        public ICustomPropertyDataAppender CustomPropertyDataAppender
        {
            get => _CustomPropertyDataAppender ?? (_CustomPropertyDataAppender = new CustomPropertyDataAppender(CustomPropertyDataFuncs));
            set => _CustomPropertyDataAppender = value;
        }
        private ICustomPropertyDataAppender _CustomPropertyDataAppender;

        public ICustomPropertyAppender CustomPropertyAppender
        {
            get => _CustomPropertyAppender ?? (_CustomPropertyAppender = new CustomPropertyAppender(CustomPropertyFuncs));
            set => _CustomPropertyAppender = value;
        } private ICustomPropertyAppender _CustomPropertyAppender;

        public ICustomPropertyFuncs CustomPropertyFuncs { get; } = new CustomPropertyFuncs();
        public ICustomPropertyDataFuncs CustomPropertyDataFuncs { get; } = new CustomPropertyDataFuncs();
        public IRelatedEntityNavigationPropertyBuilder RelatedEntityNavigationPropertyBuilder
        {
            get => _RelatedEntityNavigationPropertyBuilder ?? (_RelatedEntityNavigationPropertyBuilder = new RelatedEntityNavigationPropertyBuilder());
            internal set => _RelatedEntityNavigationPropertyBuilder = value;
        } private IRelatedEntityNavigationPropertyBuilder _RelatedEntityNavigationPropertyBuilder;

        public IRelatedEntityForeignNavigationPropertyBuilder RelatedEntityForeignNavigationPropertyBuilder
        {
            get => _EntityForeignNavigationPropertyBuilder ?? (_EntityForeignNavigationPropertyBuilder = new RelatedEntityForeignNavigationPropertyBuilder(CustomPropertyDataAppender));
            internal set => _EntityForeignNavigationPropertyBuilder = value;
        } private IRelatedEntityForeignNavigationPropertyBuilder _EntityForeignNavigationPropertyBuilder;

        public IRelatedEntityMappingNavigationPropertyBuilder RelatedEntityMappingNavigationPropertyBuilder
        {
            get => _EntityMappingNavigationPropertyBuilder ?? (_EntityMappingNavigationPropertyBuilder = new RelatedEntityMappingNavigationPropertyBuilder());
            internal set => _EntityMappingNavigationPropertyBuilder = value;
        } private IRelatedEntityMappingNavigationPropertyBuilder _EntityMappingNavigationPropertyBuilder;
        #endregion
    }
}