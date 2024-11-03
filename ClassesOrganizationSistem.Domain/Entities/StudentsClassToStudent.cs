using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Domain.Entities
{
    [PrimaryKey(nameof(StudentsClassId), nameof(StudentId))]
    public class StudentsClassToStudent
    {
        [Required]
        public int StudentsClassId { get; set; }

        [Required]
        public int StudentId { get; set; }

        public StudentsClass StudentsClass { get; set; } = null!;

        public User Student { get; set; } = null!;
    }
}
