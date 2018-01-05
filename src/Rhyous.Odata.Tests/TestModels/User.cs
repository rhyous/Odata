namespace Rhyous.Odata.Tests
{
    [RelatedEntityForeign("UserGroup", "UserGroupMembership", "User", GetAll = true)]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
    }
}