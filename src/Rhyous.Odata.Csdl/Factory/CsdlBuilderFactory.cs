using System;

namespace Rhyous.Odata.Csdl
{
    public class CsdlBuilderFactory : ICsdlBuilderFactory
    {
        #region Singleton

        private static readonly Lazy<CsdlBuilderFactory> Lazy = new Lazy<CsdlBuilderFactory>(() => new CsdlBuilderFactory());

        internal CsdlBuilderFactory()
        {
            CsdlTypeDictionary = new CsdlTypeDictionary();
            CustomPropertyDataFuncs = new CustomPropertyDataFuncs();
            CustomPropertyFuncs = new CustomPropertyFuncs();
            MaxLengthAttributeDictionary = new MaxLengthAttributeDictionary();
            MinLengthAttributeDictionary = new MinLengthAttributeDictionary();
            PropertyDataAttributeDictionary = new PropertyDataAttributeDictionary();
            RelatedEntityNavigationPropertyBuilder = new RelatedEntityNavigationPropertyBuilder();
            RelatedEntityMappingNavigationPropertyBuilder = new RelatedEntityMappingNavigationPropertyBuilder();
            CustomPropertyDataAppender = new CustomPropertyDataAppender(CustomPropertyDataFuncs);
            RelatedEntityForeignNavigationPropertyBuilder = new RelatedEntityForeignNavigationPropertyBuilder(CustomPropertyDataAppender);
            EntityAttributeDictionary = new EntityAttributeDictionary(RelatedEntityForeignNavigationPropertyBuilder, RelatedEntityMappingNavigationPropertyBuilder);
            PropertyAttributeDictionary = new PropertyAttributeDictionary(RelatedEntityNavigationPropertyBuilder);
            CustomCsdlFromAttributeAppender = new CustomCsdlFromAttributeAppender(EntityAttributeDictionary, PropertyAttributeDictionary, PropertyDataAttributeDictionary);
            PropertyBuilder = new PropertyBuilder(CustomCsdlFromAttributeAppender, CustomPropertyDataAppender, CsdlTypeDictionary, MinLengthAttributeDictionary, MaxLengthAttributeDictionary);
            EnumPropertyBuilder = new EnumPropertyBuilder(CustomCsdlFromAttributeAppender, CustomPropertyDataAppender, CsdlTypeDictionary);
            CustomPropertyAppender = new CustomPropertyAppender(CustomPropertyFuncs);
            EntityBuilder = new EntityBuilder(PropertyBuilder, EnumPropertyBuilder, CustomCsdlFromAttributeAppender, CustomPropertyAppender);            
        }

        /// <summary>This singleton instance</summary>
        public static CsdlBuilderFactory Instance { get { return Lazy.Value; } }

        #endregion

        /// <summary>A constructor so you can build your own factory</summary>
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
                                  IRelatedEntityMappingNavigationPropertyBuilder relatedEntityMappingNavigationPropertyBuilder,
                                  IMinLengthAttributeDictionary minLengthAttributeDictionary,
                                  IMaxLengthAttributeDictionary maxLengthAttributeDictionary
                                  )
        {
            EntityBuilder = entityBuilder;
            PropertyBuilder = propertyBuilder;
            EnumPropertyBuilder = enumPropertyBuilder;
            CsdlTypeDictionary = csdlTypeDictionary;
            EntityAttributeDictionary = entityAttributeDictionary;
            PropertyAttributeDictionary = propertyAttributeDictionary;
            PropertyDataAttributeDictionary = propertyDataAttributeDictionary;
            CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            CustomPropertyFuncs = customPropertyFuncs;
            CustomPropertyDataFuncs = customPropertyDataFuncs;
            RelatedEntityNavigationPropertyBuilder = relatedEntityNavigationPropertyBuilder;
            RelatedEntityForeignNavigationPropertyBuilder = relatedEntityForeignNavigationPropertyBuilder;
            RelatedEntityMappingNavigationPropertyBuilder = relatedEntityMappingNavigationPropertyBuilder;
            MinLengthAttributeDictionary = minLengthAttributeDictionary;
            MaxLengthAttributeDictionary = maxLengthAttributeDictionary;
        }

        #region Builders
        public IEntityBuilder EntityBuilder { get; }

        public IPropertyBuilder PropertyBuilder { get; }

        public IEnumPropertyBuilder EnumPropertyBuilder { get; }

        public ICsdlTypeDictionary CsdlTypeDictionary { get; }
        #endregion


        #region Attribute Dictionaries

        public IEntityAttributeDictionary EntityAttributeDictionary { get; }

        public IPropertyAttributeDictionary PropertyAttributeDictionary { get; }

        public IPropertyDataAttributeDictionary PropertyDataAttributeDictionary { get; }

        public IMinLengthAttributeDictionary MinLengthAttributeDictionary { get; }
        public IMaxLengthAttributeDictionary MaxLengthAttributeDictionary { get; }
        #endregion

        #region Custom Builders

        public ICustomCsdlFromAttributeAppender CustomCsdlFromAttributeAppender { get; }

        public ICustomPropertyDataAppender CustomPropertyDataAppender { get; }

        public ICustomPropertyAppender CustomPropertyAppender { get; }

        public ICustomPropertyFuncs CustomPropertyFuncs { get; }
        public ICustomPropertyDataFuncs CustomPropertyDataFuncs { get; }
        public IRelatedEntityNavigationPropertyBuilder RelatedEntityNavigationPropertyBuilder { get; }

        public IRelatedEntityForeignNavigationPropertyBuilder RelatedEntityForeignNavigationPropertyBuilder { get; }

        public IRelatedEntityMappingNavigationPropertyBuilder RelatedEntityMappingNavigationPropertyBuilder { get; }
        #endregion
    }
}