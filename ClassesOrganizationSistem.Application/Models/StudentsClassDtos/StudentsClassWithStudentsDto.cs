using ClassesOrganizationSistem.Application.Models.UserDtos;
using ClassesOrganizationSistem.Domain.Entities;

namespace ClassesOrganizationSistem.Application.Models.StudentsClassDtos
{
    public class StudentsClassWithStudentsDto : StudentsClassDto
    {
        public IEnumerable<UserDto> Students { get; set; }
            = new List<UserDto>();  
        
        public static new StudentsClassWithStudentsDto MapFromStudentsClass(
            StudentsClass studentsClass)
        {
            return new StudentsClassWithStudentsDto
            {
                Id = studentsClass.Id,
                Title = studentsClass.Title,
                SchoolId = studentsClass.SchoolId,
                Students = studentsClass.Students
                    .Select(UserDto.MapFromUser)
            };
        }
    }
}
