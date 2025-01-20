namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class RoleInSchoolDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public static RoleInSchoolDto MapFromRoleInSchool(RoleInSchoolDto role)
        {
            return new RoleInSchoolDto()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}
