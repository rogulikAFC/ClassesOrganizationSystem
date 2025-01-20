using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    public class Role : IdentityRole<int>
    {
        public override string Name { get; set; } = null!;

        public ICollection<UsersToRoles> UsersToRole { get; set; }
            = new List<UsersToRoles>();

        public ICollection<User> Users =>
            UsersToRole
                .Select(userToRole => userToRole.User)
                .ToList();
    }
}
