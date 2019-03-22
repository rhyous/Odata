namespace Rhyous.Odata.Csdl
{
    public interface ICsdlBuilderFactory
    {
        // Builders
        EntityBuilder EntityBuilder { get; }
        PropertyBuilder PropertyBuilder { get; }
        EnumPropertyBuilder EnumPropertyBuilder { get; }

        // Dictionaries
        CustomPropertyDataFuncs CustomPropertyDataFuncs { get; }
        EntityAttributeDictionary EntityAttributeDictionary { get; }
        PropertyAttributeDictionary PropertyAttributeDictionary { get; }
        PropertyDataAttributeDictionary PropertyDataAttributeDictionary { get; }
    }
}