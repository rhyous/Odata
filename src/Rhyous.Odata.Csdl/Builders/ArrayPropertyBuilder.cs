using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public class ArrayPropertyBuilder : IArrayPropertyBuilder
    {
        private readonly ICustomCsdlFromAttributeAppender _CustomCsdlFromAttributeAppender;
        private readonly ICustomPropertyDataAppender _CustomPropertDataAppender;
        private readonly ICsdlTypeDictionary _CsdlTypeDictionary;

        public ArrayPropertyBuilder(ICustomCsdlFromAttributeAppender customCsdlFromAttributeAppender,
                                    ICustomPropertyDataAppender customPropertDataAppender,
                                    ICsdlTypeDictionary csdlTypeDictionary)
        {
            _CustomCsdlFromAttributeAppender = customCsdlFromAttributeAppender;
            _CustomPropertDataAppender = customPropertDataAppender;
            _CsdlTypeDictionary = csdlTypeDictionary;
        }

        public CsdlArrayProperty Build(PropertyInfo propInfo)
        {
            if (propInfo == null)
                return null;
            var propertyType = propInfo.PropertyType;
            var csdlPropAttribute = propInfo.GetAttributeWithInterfaceInheritance<CsdlPropertyAttribute>();
            var elementType = propertyType.GetElementType();
            var elementTypeName = elementType.FullName;
            var csdlType = csdlPropAttribute?.CsdlType;
            if (string.IsNullOrWhiteSpace(csdlType) && !_CsdlTypeDictionary.TryGetValue(elementTypeName, out csdlType))
                return null;
            var isNullable = propInfo.IsNullable(csdlPropAttribute);
            var prop = new CsdlArrayProperty
            {
                Type = $"Collection({csdlType})",
                IsCollection = true,
                Nullable = isNullable,
                DefaultValue = csdlPropAttribute?.DefaultValue,
            };
            _CustomPropertDataAppender.Append(prop.CustomData, propInfo.ReflectedType.Name, propInfo.Name);
            _CustomCsdlFromAttributeAppender.AppendPropertyDataFromAttributes(prop.CustomData, propInfo);
            RemoveIsRequiredIfItMatchesNullable(prop);
            return prop;
        }

        private static void RemoveIsRequiredIfItMatchesNullable(CsdlProperty prop)
        {
            if (prop.CustomData.TryGetValue(CsdlConstants.UIRequired, out object isRequired))
            {
                if ((bool)isRequired != prop.Nullable)
                    prop.CustomData.TryRemove(CsdlConstants.UIRequired, out _);
            }
        }
    }
}
