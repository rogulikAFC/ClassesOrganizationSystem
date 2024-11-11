using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.Models.SchoolDtos
{
    public class SchoolDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Address { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public static SchoolDto MapFromSchool(School school)
        {
            return new SchoolDto
            {
                Id = school.Id,
                Title = school.Title,
                Address = school.Address,
                Phone = school.Phone,
                Email = school.Email
            };
        }
    }
}
