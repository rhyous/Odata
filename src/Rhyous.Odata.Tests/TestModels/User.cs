namespace Rhyous.Odata.Tests
{
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
    }
}