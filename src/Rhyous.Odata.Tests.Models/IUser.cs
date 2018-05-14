namespace Rhyous.Odata.Tests
{
    public interface IUser
    {
        int Id { get; set; }
        string Name { get; set; }
        int UserTypeId { get; set; }
    }
}