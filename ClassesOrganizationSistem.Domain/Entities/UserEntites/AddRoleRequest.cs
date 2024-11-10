using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Domain.Entities.UserEntites
{
    [PrimaryKey(nameof(UserId), nameof(SchoolId), nameof(RoleId))]
    public class AddRoleRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int SchoolId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public User User { get; set; } = null!;

        public School School { get; set; } = null!;

        public SchoolRole Role { get; set; } = null!;
    }
}
