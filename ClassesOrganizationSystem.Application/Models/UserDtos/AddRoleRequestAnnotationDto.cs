using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class AddRoleRequestAnnotationDto
    {
        public int Id { get; set; }

        public UserAnnotationDto User { get; set; } = null!;

        public string Role { get; set; } = null!;

        public static AddRoleRequestAnnotationDto MapFromAddRoleRequest(
            AddRoleRequest request)
        {
            return new AddRoleRequestAnnotationDto
            {
                Id = request.Id,
                User = UserAnnotationDto.MapFromUser(request.User),
                Role = request.Role.Name,
            };
        }
    }
}
