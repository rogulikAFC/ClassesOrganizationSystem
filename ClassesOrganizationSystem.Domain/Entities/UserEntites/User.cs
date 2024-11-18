using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Domain.Entities.UserEntites
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public string Surname { get; set; } = null!;

        public string FullName => Name + " " + Surname;

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        [Required]
        [EmailAddress]
        public override string Email { get; set; } = null!;

        [Required]
        [Phone]
        public override string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public override string UserName { get; set; } = null!;

        public IEnumerable<UserRoleInSchool> SchoolRoles { get; set; }
            = new List<UserRoleInSchool>();

        public IEnumerable<StudentsClassToStudent> StudentsClassesToStudents
            = new List<StudentsClassToStudent>();

        public IEnumerable<StudentsClass> StudentsClasses
            => StudentsClassesToStudents.Select(studentsClassToStudent =>
                studentsClassToStudent.StudentsClass);

        public IEnumerable<School> Schools => 
            SchoolRoles.Select(userRoleInSchool => userRoleInSchool.School);

        public IEnumerable<string> GetUserRolesInSchool(School school) =>  
            SchoolRoles
                .Where(userRoleInSchool => userRoleInSchool.School == school)
                .Select(userRoleInSchool => userRoleInSchool.SchoolRole.Name);

        public bool IsUserAdminInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("admin");

        public bool IsUserTeacherInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("teacher");

        public bool IsUserStudentInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("student");
    }
}
