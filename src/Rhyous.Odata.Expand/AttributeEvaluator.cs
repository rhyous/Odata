using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.Odata.Expand
{
    /// <summary>
    /// Evalutes the Attributes against the $expand parameter and determines which related entities to expand.
    /// </summary>
    /// <remarks>This doesn't use an interface because everything is already passed in to this class. It is already safe.</remarks>
    public class AttributeEvaluator
    {
        /// <summary>
        /// Looks for RelatedEntity attributes to expand.
        /// </summary>
        /// <param name="entityType">The type that might have an Attribute of type IRelatedEntity applied to the class.</param>
        /// <param name="entitiesToExpand"></param>
        /// <returns>A list of Attributes of type IRelatedEntity to expand.</returns>
        public IEnumerable<TAttribute> GetAttributesToExpand<TAttribute>(Type entityType, IEnumerable<string> entitiesToExpand = null)
            where TAttribute : Attribute, IRelatedEntityAttribute
        {
            var classAttribs = entityType.GetCustomAttributes<TAttribute>();
            var propAttribs = entityType.GetProperties().SelectMany(p => p.GetCustomAttributes<TAttribute>(true));
            var attribs = classAttribs.Concat(propAttribs);
            return GetAttributesToExpand(entitiesToExpand, attribs);
        }

        /// <summary>
        /// Looks for an attribute to expand where the attribute implements IRelatedEntity.
        /// </summary>
        /// <param name="t">The actually attribute type of the attribute that implements IRelatedEntity.</param>
        /// <param name="entitiesToExpand"></param>
        /// <param name="attribs">The attributes.</param>
        /// <returns>A list of T which is a list of an attribute that implements IRelatedEntity.</returns>
        public IEnumerable<TAttribute> GetAttributesToExpand<TAttribute>(IEnumerable<string> entitiesToExpand, IEnumerable<TAttribute> attribs)
            where TAttribute : Attribute, IRelatedEntityAttribute
        {
            var safeAttribs = attribs.Where(a => a != null);
            if (entitiesToExpand == null || !entitiesToExpand.Any())
                return safeAttribs.Where(a =>a.AutoExpand);
            else
                return safeAttribs.Where(a => (entitiesToExpand.Contains(a.RelatedEntity) && string.IsNullOrWhiteSpace(a.RelatedEntityAlias)) 
                                           || entitiesToExpand.Contains(a.RelatedEntityAlias)
                                           || entitiesToExpand.Contains(ExpandConstants.WildCard));
        }
    }
}