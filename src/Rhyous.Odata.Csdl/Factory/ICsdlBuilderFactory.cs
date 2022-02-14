namespace Rhyous.Odata.Csdl
{
    public interface ICsdlBuilderFactory
    {
        ICsdlTypeDictionary CsdlTypeDictionary { get; }
        ICustomCsdlFromAttributeAppender CustomCsdlBuilder { get; set; }
        ICustomPropertyDataFuncs CustomPropertyDataFuncs { get; }
        ICustomPropertyFuncs CustomPropertyFuncs { get; }
        IEntityAttributeDictionary EntityAttributeDictionary { get; }
        IEntityBuilder EntityBuilder { get; }
        IEnumPropertyBuilder EnumPropertyBuilder { get; }
        IPropertyAttributeDictionary PropertyAttributeDictionary { get; }
        IPropertyBuilder PropertyBuilder { get; }
        IPropertyDataAttributeDictionary PropertyDataAttributeDictionary { get; }
        IRelatedEntityForeignNavigationPropertyBuilder RelatedEntityForeignNavigationPropertyBuilder { get; }
        IRelatedEntityMappingNavigationPropertyBuilder RelatedEntityMappingNavigationPropertyBuilder { get; }
        IRelatedEntityNavigationPropertyBuilder RelatedEntityNavigationPropertyBuilder { get; }
    }
}