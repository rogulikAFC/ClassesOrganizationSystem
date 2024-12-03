using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    public class AddRoleRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
