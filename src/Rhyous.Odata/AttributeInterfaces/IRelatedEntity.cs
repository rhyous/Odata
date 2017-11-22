namespace Rhyous.Odata
{
    public interface IRelatedEntity
    {
        /// <summary>
        /// The name of the related entity.
        /// </summary>
        string RelatedEntity { get; set; }

        /// <summary>
        /// If this is true, a related entity will autoexpand. If false, it will only expand if the $expand Url parameters
        /// is used. If $expand is used, AutoExpand is ignored, so even AutoExpand related entities must be specified.
        /// is passed to the web service.
        /// </summary>
        bool AutoExpand { get; set; }

        /// <summary>
        /// This is used for mapping entities with small numbers. It is faster to get all once. Also, this is only used when 
        /// returning a OdataObjectCollection. When this value is true, related entities will be included in the collection
        /// of related entities in the OdataObjectCollection, and will not be nested. This results in much smaller json
        /// serialization. 
        /// </summary>
        /// <remarks>This is ignored if getting a single entity.</remarks>
        /// <example>Imagine a UserType entity with only 10 total types. If a query returned 100 users with N UserTypes, then if
        /// the UserType was nested under each user, the json would include 100 * N instances of a related UserType in the json.
        /// So instead, return all UserTypes once. Obviously if the forign table is large, don't use this. Large, of course, is
        /// subjective. It is up to the developer to determine if this setting adds value.</example>
        bool GetAll { get; set; }
    }
}
