using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public List<SchoolAnnotationDto> Schools { get; set; }
            = new List<SchoolAnnotationDto>();

        public List<string> Roles { get; set; } = 
            new List<string>();

        public List<UserRoleInSchoolForUserDto> RolesInSchools { get; set; } =
            new List<UserRoleInSchoolForUserDto>();

        public static UserDto MapFromUser(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,

                Schools = user.Schools
                    .Select(SchoolAnnotationDto.MapFromSchool)
                    .ToList(),

                Roles = user.Roles
                    .Select(user => user.Name)
                    .ToList(),

                RolesInSchools = user.SchoolRoles
                    .Select(UserRoleInSchoolForUserDto.MapFromUserRoleInSchool)
                    .ToList(),
            };
        }
    }
}
