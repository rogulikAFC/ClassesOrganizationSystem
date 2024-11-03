using ClassesOrganizationSistem.Domain.Entities;

namespace ClassesOrganizationSistem.Application.Models.StudentsClassDtos
{
    public class StudentsClassDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public int SchoolId { get; set; }

        public static StudentsClassDto MapFromStudentsClass(
            StudentsClass studentsClass)
        {
            return new StudentsClassDto
            {
                Id = studentsClass.Id,
                Title = studentsClass.Title,
                SchoolId = studentsClass.SchoolId,
            };
        }
    }
}
