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

        public IEnumerable<UserToSchool> SchoolsToUser
            = new List<UserToSchool>();

        public IEnumerable<School> Schools => 
            SchoolsToUser.Select(schoolToUser => schoolToUser.School);

        public IEnumerable<string> GetUserRolesInSchool(School school) =>  
            SchoolRoles
                .Where(schoolRole => schoolRole.School == school)
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
