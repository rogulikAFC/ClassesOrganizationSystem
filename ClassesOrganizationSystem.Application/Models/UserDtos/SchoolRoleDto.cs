using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class SchoolRoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public static SchoolRoleDto MapFromSchoolRole(SchoolRole schoolRole)
        {
            return new SchoolRoleDto
            {
                Id = schoolRole.Id,
                Name = schoolRole.Name
            };
        }
    }
}
