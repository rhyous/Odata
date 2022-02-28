using Rhyous.Odata.Csdl;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>An IFilterConverter{TEntity} implementation that gets related entity data and uses that to convert the Filter{TEntity}.</summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class RelatedEntityFilterConverter<TEntity> : IRelatedEntityFilterConverter<TEntity>
    {
        private readonly CsdlSchema _CsdlSchema;
        private readonly IRelatedEntityFilterDataProvider _RelatedEntityProvider;

        /// <summary>An IFilterConverter{TEntity} implementation that gets related entity data and uses that to convert the Filter{TEntity}.</summary>

        /// <param name="csdlSchema">The csdlSchema for all entities.</param>
        /// <param name="relatedEntityProvider">An implementation of IRelatedEntityProvider. This is not implemented for you.</param>
        public RelatedEntityFilterConverter(CsdlSchema csdlSchema,
                                            IRelatedEntityFilterDataProvider relatedEntityProvider)
        {
            _CsdlSchema = csdlSchema;
            _RelatedEntityProvider = relatedEntityProvider;
        }

        /// <summary>
        /// Converts a Filter using Related Entity data using a dot syntax, where the Related Entity is first part and the property is the
        /// second part. So if you have an Organization entity with an Id and Name, and User entity with an OrganizationId property, you
        /// could Filter on User with the following: Organization.Name eq 'My Org'
        /// If the Organization "My Org" had an Id of 5027, then the filter would convert to this: OrganizationId eq 5027
        /// All the users for Organization 5027 "My Org" would be returned.
        /// </summary>
        /// <param name="filter">A Filter with a syntax designed to first get data from a RelatedEntity.</param>
        /// <returns>A filter for the current entity.</returns>
        /// <remarks>In this iteration, it must be a direct RelatedEntity with a local reference property, not a RelatedEntityForiegn or Mapping.</remarks>
        public bool CanConvert(Filter<TEntity> filter)
        {
            return filter != null
                && _CsdlSchema != null
                && _RelatedEntityProvider != null
                && DotSyntaxIsValid(filter)
                && IsCsdlSchemaValid(filter);
        }

        /// <inheritdoc />
        public async Task<Filter<TEntity>> ConvertAsync(Filter<TEntity> filter)
        {
            if (!CanConvert(filter))
                return null;
            var newFilter = filter.Clone(true, false);
            var relatedEntityName = filter.Left.NonFilter.Split('.')[0];
            newFilter.Left = filter.Left.NonFilter.Split('.')[1];            
            var urlParameters = $"$Filter={newFilter}";
            var relatedEntities = await _RelatedEntityProvider.ProvideAsync(relatedEntityName, urlParameters);
            if (relatedEntities == null || !relatedEntities.Any())
                return "1 eq 0";
            if (!_CsdlSchema.Entities.TryGetValue(typeof(TEntity).Name, out object objCsdl))
                return null;
            if (!(objCsdl as CsdlEntity).Properties.TryGetValue(filter.Left.NonFilter.Split('.')[0], out object objCsdlProperty))
                return null;

            var relatedEntityForeignProperty = (objCsdlProperty as CsdlNavigationProperty).ReferentialConstraint.ForeignProperty;
            var localPropertyValues = relatedEntities.Select(re => re.Object.GetValue(relatedEntityForeignProperty).ToString()).Distinct().ToArray();
            var localProperty = (objCsdlProperty as CsdlNavigationProperty).ReferentialConstraint.LocalProperty;
            if ((objCsdl as CsdlEntity).Properties.TryGetValue(localProperty, out object csdlLocalProperty))
            {
                var type = (csdlLocalProperty as CsdlProperty).Type;
                if (type == CsdlConstants.EdmString)
                {
                    localPropertyValues = localPropertyValues.Select(v => !v.IsQuoted() && v.HasWhitespace() ? v.EscapeAndQuote() : v).ToArray();
                    return new Filter<TEntity>
                    {
                        Left = $"{localProperty}",
                        Method = "in",
                        Right = new ArrayFilter<TEntity, string> { Array = localPropertyValues }
                    };
                }
            }
            Filter<TEntity> convertedFilter = $"{localProperty} in ({string.Join(",", localPropertyValues)})";
            return convertedFilter;
        }

        /// <summary>Makes sure the synax is correct: {RelatedEntity}.{Property}</summary>
        /// <param name="filter">The Filter.</param>
        /// <returns>True if the dot syntax is correct, false otherwise.</returns>
        private static bool DotSyntaxIsValid(Filter<TEntity> filter)
        {
            return filter.IsComplete
                && !filter.IsSimpleString
                && !filter.IsArray
                && filter.Left != null
                && filter.Left.IsSimpleString
                && filter.Left.NonFilter.Count(s => s == '.') == 1
                && filter.Left.NonFilter.IndexOf('.') != 0 // The . is not the first character
                && filter.Left.NonFilter.IndexOf('.') != filter.Left.NonFilter.Length - 1; // The . is not the last character;
        }

        /// <summary>Makes sure the Schema is valid and that the schema is for a Related Entity.</summary>
        /// <param name="filter">The Filter.</param>
        /// <returns>True if the schema is valid and is for a related entity. False otherwise.</returns>
        private bool IsCsdlSchemaValid(Filter<TEntity> filter)
        {
            return _CsdlSchema.Entities.TryGetValue(typeof(TEntity).Name, out object objCsdl)
                && (objCsdl as CsdlEntity) != null
                && (objCsdl as CsdlEntity).Properties.TryGetValue(filter.Left.NonFilter.Split('.')[0], out object objCsdlProperty)
                && (objCsdlProperty as CsdlNavigationProperty) != null
                && (objCsdlProperty as CsdlNavigationProperty).Kind == CsdlConstants.NavigationProperty
                && (objCsdlProperty as CsdlNavigationProperty).CustomData.TryGetValue(CsdlConstants.EAFRelatedEntityType, out object value)
                && value.ToString() == CsdlConstants.Local;
        }
    }
}