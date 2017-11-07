namespace Rhyous.Odata
{
    public interface IRelatedEntity
    {
        string RelatedEntity { get; set; }
        bool AutoExpand { get; set; }
    }
}
