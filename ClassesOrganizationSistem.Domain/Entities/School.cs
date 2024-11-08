using ClassesOrganizationSistem.Domain.Entities.RoomEntities;
using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSistem.Domain.Entities
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

        public IEnumerable<UserToSchool> UsersToSchool { get; set; }
            = new List<UserToSchool>();

        public IEnumerable<User> Users => UsersToSchool
            .Select(userToSchool => userToSchool.User);
    }
}
