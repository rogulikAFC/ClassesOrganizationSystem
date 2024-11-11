using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    [PrimaryKey(nameof(UserId), nameof(SchoolId))]
    public class UserToSchool
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int SchoolId { get; set; }

        public User User { get; set; } = null!;

        public School School { get; set; } = null!;
    }
}