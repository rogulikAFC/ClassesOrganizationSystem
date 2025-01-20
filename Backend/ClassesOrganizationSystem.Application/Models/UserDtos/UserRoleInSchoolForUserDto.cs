using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class UserRoleInSchoolForUserDto
    {
        public SchoolAnnotationDto School { get; set; } = null!;

        public string Role { get; set; } = null!;

        public static UserRoleInSchoolForUserDto MapFromUserRoleInSchool(
            UserRoleInSchool userRoleInSchool)
        {
            return new UserRoleInSchoolForUserDto
            {
                School = SchoolAnnotationDto.MapFromSchool(userRoleInSchool.School),
                Role = userRoleInSchool.SchoolRole.Name
            };
        }
    }
}
