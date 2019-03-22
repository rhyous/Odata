namespace Rhyous.Odata.Csdl
{
    public class CsdlBuilderFactory : ICsdlBuilderFactory
    {


        public CsdlBuilderFactory() { }

        public CsdlBuilderFactory(PropertyDataAttributeDictionary propertyDataAttributeDictionary,
                                  CustomPropertyDataFuncs customPropertyDataFuncs,
                                  EntityAttributeDictionary entityAttributeDictionary,
                                  PropertyAttributeDictionary propertyAttributeDictionary
                                  )
        {
            propertyDataAttributeDictionary = PropertyDataAttributeDictionary;
            CustomPropertyDataFuncs = customPropertyDataFuncs;
            entityAttributeDictionary = EntityAttributeDictionary;
            propertyAttributeDictionary = PropertyAttributeDictionary;
        }

        // Builders
        public EntityBuilder EntityBuilder
        {
            get => _EntityBuilder ?? (_EntityBuilder = CreateEntityBuilder());
            internal set => _EntityBuilder = value;
        } private EntityBuilder _EntityBuilder;

        public PropertyBuilder PropertyBuilder
        {
            get => _PropertyBuilder ?? (_PropertyBuilder = CreatePropertyBuilder());
            internal set => _PropertyBuilder = value;
        } private PropertyBuilder _PropertyBuilder;

        public EnumPropertyBuilder EnumPropertyBuilder
        {
            get => _EnumPropertyBuilder ?? (_EnumPropertyBuilder = CreateEnumPropertyBuilder());
            internal set => _EnumPropertyBuilder = value;
        } private EnumPropertyBuilder _EnumPropertyBuilder;

        // Dictionaries - set by default
        public PropertyDataAttributeDictionary PropertyDataAttributeDictionary { get; } = new PropertyDataAttributeDictionary();
        public CustomPropertyDataFuncs CustomPropertyDataFuncs { get; } = new CustomPropertyDataFuncs();
        public EntityAttributeDictionary EntityAttributeDictionary { get; } = new EntityAttributeDictionary();
        public PropertyAttributeDictionary PropertyAttributeDictionary { get; } = new PropertyAttributeDictionary();

        internal EntityBuilder CreateEntityBuilder() => new EntityBuilder(PropertyBuilder, EnumPropertyBuilder, EntityAttributeDictionary, PropertyAttributeDictionary);
        internal PropertyBuilder CreatePropertyBuilder() => new PropertyBuilder(PropertyDataAttributeDictionary, CustomPropertyDataFuncs);
        internal EnumPropertyBuilder CreateEnumPropertyBuilder() => new EnumPropertyBuilder(PropertyDataAttributeDictionary, CustomPropertyDataFuncs);
    }
}