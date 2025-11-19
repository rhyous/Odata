using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Expand
{
    public interface IAttributeEvaluator
    {
        /// <summary>Looks for RelatedEntity attributes to expand.</summary>
        /// <param name="entityType">The type that might have an Attribute of type IRelatedEntity applied to the class.</param>
        /// <param name="entitiesToExpand"></param>
        /// <returns>A list of Attributes of type IRelatedEntity to expand.</returns>
        IEnumerable<TAttribute> GetAttributesToExpand<TAttribute>(Type entityType, IEnumerable<string> entitiesToExpand = null)
            where TAttribute : Attribute, IRelatedEntityAttribute;

        /// <summary>Looks for an attribute to expand where the attribute implements IRelatedEntity.</summary>
        /// <param name="t">The actually attribute type of the attribute that implements IRelatedEntity.</param>
        /// <param name="entitiesToExpand"></param>
        /// <param name="attribs">The attributes.</param>
        /// <returns>A list of T which is a list of an attribute that implements IRelatedEntity.</returns>
        IEnumerable<TAttribute> GetAttributesToExpand<TAttribute>(IEnumerable<string> entitiesToExpand, IEnumerable<TAttribute> attribs)
            where TAttribute : Attribute, IRelatedEntityAttribute;
    }
}