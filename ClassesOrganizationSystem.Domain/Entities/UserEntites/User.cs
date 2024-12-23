using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string FullName => Name + " " + Surname;

        [Required]
        [EmailAddress]
        public override string Email { get; set; } = null!;

        [Required]
        [Phone]
        public override string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(32)]
        public override string UserName { get; set; } = null!;

        public List<UsersToRoles> RolesToUser { get; set; }
            = new List<UsersToRoles>();

        public List<Role> Roles => 
            RolesToUser
                .Select(rolesToUser => rolesToUser.Role)
                .ToList();

        public List<UserRoleInSchool> SchoolRoles { get; set; }
            = new List<UserRoleInSchool>();

        public List<StudentsClassToStudent> StudentsClassesToStudents
            = new List<StudentsClassToStudent>();

        [NotMapped]
        public List<StudentsClass> StudentsClasses
            => StudentsClassesToStudents.Select(studentsClassToStudent =>
                studentsClassToStudent.StudentsClass)
            .ToList();

        [NotMapped]
        public List<School> Schools => 
            SchoolRoles.Select(userRoleInSchool => userRoleInSchool.School)
            .ToList();

        public List<string> GetUserRolesInSchool(School school) =>  
            SchoolRoles
                .Where(userRoleInSchool => userRoleInSchool.School == school)
                .Select(userRoleInSchool => userRoleInSchool.SchoolRole.Name)
            .ToList();

        public bool IsAdminInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("admin");

        public bool IsTeacherInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("teacher");

        public bool IsStudentInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Contains("student");

        public bool IsUserInSchool(School school) =>
            GetUserRolesInSchool(school)
            .Any();
    }
}
