namespace Rhyous.Odata
{
    /// <summary>
    /// An interface for the minimal related entity attributes.
    /// </summary>
    public interface IRelatedEntityAttribute
    {
        /// <summary>
        /// The name of the current entity.
        /// </summary>
        string Entity { get; set; }
        
        /// <summary>
        /// An alias for the current entity. This is useful when an entity is related to the same Entity in two ways.
        /// </summary>
        string EntityAlias { get; set; }

        /// <summary>
        /// The name of the related entity.
        /// </summary>
        string RelatedEntity { get; set; }
        
        /// <summary>
        /// An alias for the related entity. This is useful when an entity is related to the same Entity in two ways.
        /// </summary>
        string RelatedEntityAlias { get; set; }

        /// <summary>
        /// If this is true, a related entity will autoexpand. If false, it will only expand if the $expand Url parameters
        /// is used. If $expand is used, AutoExpand is ignored, so even AutoExpand related entities must be specified.
        /// is passed to the web service.
        /// </summary>
        bool AutoExpand { get; set; }

        /// <summary>
        /// This is used for mapping entities with small numbers. It is faster to get all once. Also, this is only used when 
        /// returning a OdataObjectCollection. When this value is true, related entities should be included in the collection
        /// of related entities in the OdataObjectCollection, and will not be nested. This results in much smaller json
        /// serialization. 
        /// </summary>
        /// <remarks>This is ignored if getting a single entity.</remarks>
        /// <example>Imagine a UserType entity with only 10 total types. If a query returned 100 users with N UserTypes, then if
        /// the UserType was nested under each user, the json would include 100 * N instances of a related UserType in the json.
        /// So instead, return all UserTypes once. Obviously if the forign table is large, don't use this. Large, of course, is
        /// subjective. It is up to the developer to determine if this setting adds value.</example>
        bool GetAll { get; set; }
        /// <summary>
        /// The ability to enforce a filter on a related entity. For example, imagine you want a related entity for an entity with
        /// a TypeId property and you only want to relate to TypeId 1. You can put the following Odata filter here:
        ///     TypeId eq 1
        /// Do not include the "$Filter=" prefix. It is the callers responsiblity to enforce use this.
        /// </summary>
        string Filter { get; set; }
        /// <summary>
        /// The ability to enforce a condition on a related entity that determines if the UI should display
        /// the related entity. For example, imagine that an entity has a TypeId and the related entity only
        /// exists for a certain type. You can put the following Odata filter here:
        ///   TypeId eq 3
        /// Do not include the "$Filter=" prefix. It is the callers responsiblity to enforce use this.
        /// If not DisplayCondition is provided, it should display by default.
        /// </summary>
        string DisplayCondition { get; set; }
    }
}
