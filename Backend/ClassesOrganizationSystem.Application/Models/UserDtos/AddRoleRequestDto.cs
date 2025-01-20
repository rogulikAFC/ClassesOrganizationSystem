using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class AddRoleRequestDto
    {
        public int Id { get; set; }

        public UserAnnotationDto User { get; set; } = null!;

        public SchoolAnnotationDto School { get; set; } = null!;

        public string Role { get; set; } = null!;

        public static AddRoleRequestDto MapFromAddRoleRequest(
            AddRoleRequest request)
        {
            return new AddRoleRequestDto
            {
                Id = request.Id,
                User = UserAnnotationDto.MapFromUser(request.User),
                School = SchoolAnnotationDto.MapFromSchool(request.School),
                Role = request.Role.Name,
            };
        }
    }
}
