using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata.Csdl.Tests.Extensions
{
    [TestClass]
    internal class TypeExtensionsTests
    {
        #region GetInterfaceAttributesNotOverridden
        [TestMethod]
        public void TypeExtensions_GetInterfaceAttributesNotOverridden_GetsAttributeFromInterface_Test()
        {
            // Arrange
            Type type = typeof(EntityWithInterfaceAttribute);
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type>();

            // Act
            var result = type.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(typeof(LookupEntityAttribute), result[0].GetType());
        }

        [TestMethod]
        public void TypeExtensions_GetInterfaceAttributesNotOverridden_AttributeFromInterface_Overridden_Test()
        {
            // Arrange
            Type type = typeof(EntityWithInterfaceAttribute);
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type> { typeof(LookupEntityAttribute) };

            // Act
            var result = type.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TypeExtensions_GetInterfaceAttributesNotOverridden_NoInterface_Test()
        {
            // Arrange
            Type type = typeof(EntityWithoutInterface);
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type> { };

            // Act
            var result = type.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }
        #endregion
    }
}
