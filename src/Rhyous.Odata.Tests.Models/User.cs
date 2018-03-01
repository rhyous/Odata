namespace Rhyous.Odata.Tests
{

    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User", AutoExpand = true)]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
    }
}