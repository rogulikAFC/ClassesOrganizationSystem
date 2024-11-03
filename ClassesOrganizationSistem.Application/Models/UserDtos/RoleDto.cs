using ClassesOrganizationSistem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSistem.Application.Models.UserDtos
{
    public class RoleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public static RoleDto MapFromRole(Role role)
        {
            return new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}
