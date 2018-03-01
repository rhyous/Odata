namespace Rhyous.Odata.Tests
{

    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User2")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User2", AutoExpand = true)]
    public class User2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity("UserType", ForeignKeyProperty = "Name")]
        public string UserTypeName { get; set; }
    }
}