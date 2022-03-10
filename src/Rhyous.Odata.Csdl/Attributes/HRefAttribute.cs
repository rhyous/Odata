using System;

namespace Rhyous.Odata.Csdl
{
    /// <summary>An attribute that marks a string property as an URL.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HRefAttribute : Attribute
    {
    }
}