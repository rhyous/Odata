using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>
    /// This decorator exists so an entity will get a @UI.ReadOnly property set to true.
    /// </summary>
    /// <remarks>We didn't find an attribute in System.ComponentModel.DataAnnotations to use.</remarks>
    public class ReadOnlyEntityAttribute : Attribute
    {
    }
}
