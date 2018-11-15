using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{

    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User", AutoExpand = true)]
    public class User : IUser
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
    }
}