namespace Rhyous.Odata.Filter
{
    /// <summary>The interface for a RelatedEntityFilterConverter.</summary>
    /// <typeparam name="TEntity">The type of Entity this converter is for.</typeparam>
    public interface IRelatedEntityFilterConverter<TEntity> : IFilterConverter<TEntity>
    {
    }
}