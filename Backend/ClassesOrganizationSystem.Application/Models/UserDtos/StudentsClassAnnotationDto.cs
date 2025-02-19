using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.Models.UserDtos
{
    public class StudentsClassAnnotationDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public static StudentsClassAnnotationDto MapFromStudentsClass(StudentsClass studentsClass)
        {
            return new StudentsClassAnnotationDto()
            {
                Id = studentsClass.Id,
                Title = studentsClass.Title,
            };
        }
    }
}
