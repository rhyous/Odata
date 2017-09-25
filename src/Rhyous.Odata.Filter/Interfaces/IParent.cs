namespace Rhyous.Odata
{
    public interface IParent<T>
    {
        T Parent { get; set; }
    }
}
