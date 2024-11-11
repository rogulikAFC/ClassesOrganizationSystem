using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.Models.SchoolDtos
{
    public class SchoolAnnotationDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public static SchoolAnnotationDto MapFromSchool(School school)
        {
            return new SchoolAnnotationDto
            {
                Id = school.Id,
                Title = school.Title,
            };
        }
    }
}
