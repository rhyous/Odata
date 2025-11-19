using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Csdl.Tests.Extensions
{
    [TestClass]
    public class PropertyInfoExtensionsTests
    {
        #region GetInterfaceAttributesNotOverridden
        [TestMethod]
        public void PropertyInfoExtensions_GetInterfaceAttributesNotOverridden_GetsAttributeFromInterfaceProperty_Test()
        {
            // Arrange
            Type type = typeof(EntityWithInterfaceAttribute);
            PropertyInfo propInfo = type.GetProperty(nameof(EntityWithInterfaceAttribute.Name));
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type>();

            // Act
            var result = propInfo.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.HasCount(1, result);
            Assert.AreEqual(typeof(StringLengthAttribute), result[0].GetType());
        }

        [TestMethod]
        public void PropertyInfoExtensions_GetInterfaceAttributesNotOverridden_GetsAttributeFromInterfaceProperty_Overridden_Test()
        {
            // Arrange
            Type type = typeof(EntityWithInterfaceAttribute);
            PropertyInfo propInfo = type.GetProperty(nameof(EntityWithInterfaceAttribute.Name));
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type> { typeof(StringLengthAttribute) };

            // Act
            var result = propInfo.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void PropertyInfoExtensions_GetInterfaceAttributesNotOverridden_NoInterface_Test()
        {
            // Arrange
            Type type = typeof(EntityWithInterfaceAttribute);
            PropertyInfo propInfo = type.GetProperty(nameof(EntityWithInterfaceAttribute.Name));
            HashSet<Type> overriddenAttributeTypes = new HashSet<Type> { typeof(StringLengthAttribute) };

            // Act
            var result = propInfo.GetInterfaceAttributesNotOverridden(overriddenAttributeTypes).ToList();

            // Assert
            Assert.IsEmpty(result);
        }
        #endregion
    }
}
