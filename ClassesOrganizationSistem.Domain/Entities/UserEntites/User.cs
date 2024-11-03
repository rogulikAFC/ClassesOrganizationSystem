using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Domain.Entities.UserEntites
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string Surname { get; set; } = null!;

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public int? SchoolId { get; set; }

        public School? School { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; } = null!;

        [Required]
        [Phone]
        public override string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public override string UserName { get; set; } = null!;

        public IEnumerable<StudentsClassToStudent> StudentsClassesToStudents
            = new List<StudentsClassToStudent>();

        public IEnumerable<StudentsClass> StudentsClasses
            => StudentsClassesToStudents.Select(studentsClassToStudent =>
                studentsClassToStudent.StudentsClass);
    }
}
