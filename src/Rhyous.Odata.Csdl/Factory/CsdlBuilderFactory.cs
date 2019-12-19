using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class CsdlBuilderFactory : ICsdlBuilderFactory
    {
        public CsdlBuilderFactory() { }

        public CsdlBuilderFactory(IFuncList<string> customPropertyFuncs) 
        {
            CustomPropertyFuncs = customPropertyFuncs;
        }

        public CsdlBuilderFactory(IFuncDictionary<Type, MemberInfo> propertyDataAttributeDictionary,
                                  IFuncDictionary<Type, MemberInfo> entityAttributeDictionary,
                                  IFuncDictionary<Type, MemberInfo> propertyAttributeDictionary,
                                  IFuncList<string> customPropertyFuncs,
                                  IFuncList<string, string> customPropertyDataFuncs,
                                  IDictionary<string, string> csdlTypeDictionary
                                  )
        {
            PropertyDataAttributeDictionary = propertyDataAttributeDictionary;
            EntityAttributeDictionary = entityAttributeDictionary;
            PropertyAttributeDictionary = propertyAttributeDictionary;
            CustomPropertyFuncs = customPropertyFuncs;
            CustomPropertyDataFuncs = customPropertyDataFuncs;
            CsdlTypeDictionary = csdlTypeDictionary;
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
        public IFuncDictionary<Type, MemberInfo> PropertyDataAttributeDictionary { get; } = new PropertyDataAttributeDictionary();
        public IFuncDictionary<Type, MemberInfo> EntityAttributeDictionary { get; } = new EntityAttributeDictionary();
        public IFuncDictionary<Type, MemberInfo> PropertyAttributeDictionary { get; } = new PropertyAttributeDictionary();
        public IFuncList<string> CustomPropertyFuncs { get; } = new CustomPropertyFuncs();
        public IFuncList<string, string> CustomPropertyDataFuncs { get; } = new CustomPropertyDataFuncs();
        public IDictionary<string,string> CsdlTypeDictionary { get; } = new CsdlTypeDictionary();

        internal EntityBuilder CreateEntityBuilder() => new EntityBuilder(PropertyBuilder, EnumPropertyBuilder, EntityAttributeDictionary, PropertyAttributeDictionary, CustomPropertyFuncs);
        internal PropertyBuilder CreatePropertyBuilder() => new PropertyBuilder(PropertyDataAttributeDictionary, CustomPropertyDataFuncs, CsdlTypeDictionary);
        internal EnumPropertyBuilder CreateEnumPropertyBuilder() => new EnumPropertyBuilder(PropertyDataAttributeDictionary, CustomPropertyDataFuncs, CsdlTypeDictionary);
    }
}