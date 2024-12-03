using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    public class UsersToRoles : IdentityUserRole<int>
    {
        [Required]
        public override int UserId { get; set; }

        [Required]
        public override int RoleId { get; set; }

        public User User { get; set; } = null!;

        public Role Role { get; set; } = null!;
    }
}
