using System;

namespace Rhyous.Odata
{
    /// <summary>This attribute should be added to any Entity that is a lookup Entity.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class LookupEntityAttribute : Attribute
    {

        /// <summary>The default maximum number of entities before this entity stops behaving as if it were an enum.</summary>
        public const int DefaultMaxCountToBehaveAsEnum = 32;
        /// <summary>The configured maximum number of entities before this entity stops behaving as if it were an enum.</summary>
        public int MaxCountToBehaveAsEnum { get; set; } = DefaultMaxCountToBehaveAsEnum;
    }
}