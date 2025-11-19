using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    /// <summary>Converts a RelatedEntity to a strongly-typed OdataObject{TEntity, TId}.</summary>
    public class RelatedEntityConverter : IRelatedEntityConverter
    {
        #region RelatedEntity Conversion
        /// <summary>Converts a RelatedEntity to a strongly-typed OdataObject{TEntity, TId}.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TId">The ID type.</typeparam>
        /// <param name="relatedEntity">The RelatedEntity to convert.</param>
        /// <returns>A strongly-typed OdataObject{TEntity, TId} or null if the input is null.</returns>
        public OdataObject<TEntity, TId> Convert<TEntity, TId>(RelatedEntity relatedEntity)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>
        {
            return relatedEntity.ToOdataObject<TEntity, TId>();
        }

        /// <summary>Converts a RelatedEntity to a strongly-typed OdataObject{TEntity, TId} using runtime types.</summary>
        /// <param name="relatedEntity">The RelatedEntity to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>A strongly-typed OdataObject{TEntity, TId} as object, or null if the input is null.</returns>
        public object Convert(RelatedEntity relatedEntity, Type entityType, Type idType)
        {
            if (relatedEntity == null)
                return null;

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            if (idType == null)
                throw new ArgumentNullException(nameof(idType));

            // Get the generic Convert method and make it specific to the provided types
            var convertMethod = typeof(RelatedEntityConverter)
                .GetMethod(nameof(Convert), new[] { typeof(RelatedEntity) })
                .MakeGenericMethod(entityType, idType);

            // Invoke the generic method
            return convertMethod.Invoke(this, new object[] { relatedEntity });
        }
        #endregion

        #region RelatedEntityCollection Conversion

        /// <summary>Converts an IEnumerable{RelatedEntity} to an IEnumerable{OdataObject{TEntity, TId}}.</summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TId">The ID type.</typeparam>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId}.</returns>
        public IEnumerable<OdataObject<TEntity, TId>> Convert<TEntity, TId>(IEnumerable<RelatedEntity> relatedEntities)
            where TId : IComparable<TId>, IComparable, IEquatable<TId>
        {
            if (relatedEntities == null)
                return null;

            return relatedEntities.Select(obj => Convert<TEntity, TId>(obj));
        }

        /// <summary>Converts an IEnumerable{RelatedEntities} to an IEnumerable{OdataObject{TEntity, TId}} using runtime types.</summary>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId} as object.</returns>
        public object Convert(IEnumerable<RelatedEntity> relatedEntities, Type entityType, Type idType)
        {
            if (relatedEntities == null)
                return null;

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            if (idType == null)
                throw new ArgumentNullException(nameof(idType));

            // Get the generic Convert method for IEnumerable and make it specific to the provided types
            var convertMethod = typeof(RelatedEntityConverter)
                .GetMethods()
                .First(m => m.Name == nameof(Convert)
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(IEnumerable<RelatedEntity>)
                    && m.IsGenericMethod)
                .MakeGenericMethod(entityType, idType);

            // Invoke the generic method
            return convertMethod.Invoke(this, new object[] { relatedEntities });
        }

        /// <summary>Converts an IEnumerable{RelatedEntities} to an IEnumerable{OdataObject{TEntity, TId}} using runtime types.</summary>
        /// <param name="relatedEntities">The RelatedEntities to convert.</param>
        /// <param name="entityType">The entity type.</param>
        /// <param name="idType">The ID type.</param>
        /// <returns>An IEnumerable of strongly-typed OdataObject{TEntity, TId} as object.</returns>
        public object Convert(RelatedEntityCollection relatedEntities, Type entityType, Type idType)
        {
            return Convert(relatedEntities.AsEnumerable(), entityType, idType);
        }
        #endregion
    }
}

