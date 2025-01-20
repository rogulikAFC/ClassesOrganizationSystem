using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.Models.StudentsClassDtos
{
    public class StudentsClassWithStudentsDto : StudentsClassDto
    {
        public IEnumerable<UserAnnotationDto> Students { get; set; }
            = new List<UserAnnotationDto>();

        public SchoolAnnotationDto School { get; set; } = null!;
        
        public static new StudentsClassWithStudentsDto MapFromStudentsClass(
            StudentsClass studentsClass)
        {
            return new StudentsClassWithStudentsDto
            {
                Id = studentsClass.Id,
                Title = studentsClass.Title,
                StudentsCount = studentsClass.Students.Count(),
                Students = studentsClass.Students
                    .Select(UserAnnotationDto.MapFromUser),
                School = SchoolAnnotationDto.MapFromSchool(studentsClass.School)
            };
        }
    }
}
