using ClassesOrganizationSistem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Application.Models.StudentsClassDtos
{
    public class CreateStudentsClassDto
    {
        [Required]
        [MaxLength(8)]
        public string Title { get; set; } = null!;

        [Required]
        public int SchoolId { get; set; }

        public IEnumerable<int> StudentsIds { get; set; }
            = new List<int>();

        public StudentsClass MapToStudentsClass()
        {
            return new StudentsClass
            {
                Title = Title,
                SchoolId = SchoolId,
            };
        }
    }
}
