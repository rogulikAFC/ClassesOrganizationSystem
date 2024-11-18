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

        public IEnumerable<Room> Rooms = new List<Room>();

        public IEnumerable<UserRoleInSchool> UsersRolesInSchool { get; set; }
            = new List<UserRoleInSchool>();

        public IEnumerable<User> Users => UsersRolesInSchool
            .Select(userRoleInSchool => userRoleInSchool.User);

        public IEnumerable<StudentsClass> StudentsClasses { get; set; } 
            = new List<StudentsClass>();
    }
}
