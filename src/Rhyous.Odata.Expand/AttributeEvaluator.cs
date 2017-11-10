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
        /// Looks for RelatedEntity attributes to expand. These are directly related entities.
        /// </summary>
        /// <param name="t">The type that might have a RelatedEntityAttribute applied to a property.</param>
        /// <param name="entitiesToExpand">The related entity names to expand.</param>
        /// <returns>A RelatedEntityAttribute list of related entities to expand.</returns>
        public IEnumerable<RelatedEntityAttribute> GetAttributesToExpand(Type t, IEnumerable<string> entitiesToExpand = null)
        {
            var attribs = t.GetProperties().Select(p => p.GetCustomAttribute<RelatedEntityAttribute>(true));
            return GetAttributesToExpand(entitiesToExpand, attribs);
        }

        /// <summary>
        /// Looks for RelatedEntity attributes to expand. These are related entities that are related through a mapping table.
        /// </summary>
        /// <param name="t">The type that might have a RelatedEntityMappingAttribute applied to the class.</param>
        /// <param name="entitiesToExpand"></param>
        /// <returns>A RelatedEntityMappingAttribute list of related entities to expand.</returns>
        public IEnumerable<RelatedEntityMappingAttribute> GetMappingAttributesToExpand(Type t, IEnumerable<string> entitiesToExpand = null)
        {
            var attribs = t.GetCustomAttributes<RelatedEntityMappingAttribute>();
            return GetAttributesToExpand(entitiesToExpand, attribs);
        }

        /// <summary>
        /// Looks for an attribute to expand where the attribute implements IRelatedEntity.
        /// </summary>
        /// <param name="t">The actually attribute type of the attribute that implements IRelatedEntity.</param>
        /// <param name="entitiesToExpand"></param>
        /// <param name="attribs">The attributes.</param>
        /// <returns>A list of T which is a list of an attribute that implements IRelatedEntity.</returns>
        public IEnumerable<T> GetAttributesToExpand<T>(IEnumerable<string> entitiesToExpand, IEnumerable<T> attribs)
            where T : IRelatedEntity
        {
            var safeAttribs = attribs.Where(a => a != null);
            if (entitiesToExpand == null || !entitiesToExpand.Any())
                return safeAttribs.Where(a => a != null && a.AutoExpand);
            else
                return safeAttribs.Where(a => entitiesToExpand.Contains(a.RelatedEntity) || entitiesToExpand.Contains(ExpandConstants.WildCard));
        }
    }
}