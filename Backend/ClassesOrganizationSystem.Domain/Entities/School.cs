using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities
{
    public class School
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = null!;

        public string? Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public ICollection<Room> Rooms = new List<Room>();

        public ICollection<UserRoleInSchool> UsersRolesInSchool { get; set; }
            = new List<UserRoleInSchool>();

        [NotMapped]
        public ICollection<User> Users => UsersRolesInSchool
            .Select(userRoleInSchool => userRoleInSchool.User)
            .ToList();

        public ICollection<StudentsClass> StudentsClasses { get; set; } 
            = new List<StudentsClass>();
    }
}
