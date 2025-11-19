using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    /// <summary>
    /// Interface for converting a RelatedEntity to a strongly-typed OdataObject{TEntity, TId}.
    /// </summary>
    public interface IRelatedEntityConverter
    {
        #region RelatedEntity Conversion
        /// <summary>
        /// Converts a RelatedEntity to a strongly-typed OdataObject{TEntity, TId}.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TId">The ID type.</typeparam>
        /// <param name="relatedEntity">The RelatedEntity to convert.</param>
        /// <returns>A strongly-typed OdataObject{TEntity, TId} or null if the input is null.</returns>
        OdataObject<TEntity, TId> Convert<TEntity, TId>(RelatedEntity relatedEntity)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>;

        /// <summary>Converts a RelatedEntity to a strongly-typed OdataObject{TEntity, TId} using runtime types.</summary>
        /// <param name="relatedEntity">The RelatedEntity to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>A strongly-typed OdataObject{TEntity, TId} as object, or null if the input is null.</returns>
        object Convert(RelatedEntity relatedEntity, Type entityType, Type idType);
        #endregion

        #region RelatedEntityCollection Conversion
        /// <summary>Converts an IEnumerable{RelatedEntity} to an IEnumerable{OdataObject{TEntity, TId}}.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TId">The ID type.</typeparam>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId}.</returns>
        IEnumerable<OdataObject<TEntity, TId>> Convert<TEntity, TId>(IEnumerable<RelatedEntity> relatedEntities)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>;

        /// <summary>Converts an IEnumerable{RelatedEntities} to an IEnumerable{OdataObject{TEntity, TId}} using runtime types.</summary>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId} as object.</returns>
        object Convert(IEnumerable<RelatedEntity> relatedEntities, Type entityType, Type idType);

        /// <summary>Converts an IEnumerable{RelatedEntities} to an IEnumerable{OdataObject{TEntity, TId}} using runtime types.</summary>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId} as object.</returns>
        object Convert(RelatedEntityCollection relatedEntities, Type entityType, Type idType);
        #endregion
    }
}

