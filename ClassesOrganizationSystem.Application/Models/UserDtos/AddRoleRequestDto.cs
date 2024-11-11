using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class AddRoleRequestDto
    {
        public UserDto User { get; set; } = null!;

        public SchoolDto School { get; set; } = null!;

        public string Role { get; set; } = null!;

        public static AddRoleRequestDto MapFromAddRoleRequest(
            AddRoleRequest request)
        {
            return new AddRoleRequestDto
            {
                User = UserDto.MapFromUser(request.User),
                School = SchoolDto.MapFromSchool(request.School),
                Role = request.Role.Name,
            };
        }
    }
}
