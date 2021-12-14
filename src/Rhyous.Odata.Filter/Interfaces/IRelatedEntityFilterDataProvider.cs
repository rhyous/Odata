using System.Threading.Tasks;

namespace Rhyous.Odata.Filter
{
    /// <summary>An interface that defines a signature for getting RelatedEntities.</summary>
    public interface IRelatedEntityFilterDataProvider
    {
        /// <summary>Gets the RelatedEntities.</summary>
        /// <param name="relatedEntityName">The related entity name.</param>
        /// <param name="filter">The URL paramters.</param>
        /// <returns>An OdataObjectCollection of related entities.</returns>
        Task<OdataObjectCollection> ProvideAsync(string relatedEntityName, string filter);
    }
}