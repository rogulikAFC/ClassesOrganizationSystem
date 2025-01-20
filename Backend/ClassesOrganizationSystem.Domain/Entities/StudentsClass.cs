using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities
{
    public class StudentsClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        public string Title { get; set; } = null!;

        public int SchoolId { get; set; }

        public School School { get; set; } = null!;

        public List<StudentsClassToStudent> StudentsClassesToStudents { get; set; }
            = new List<StudentsClassToStudent>();

        public List<User> Students
            => StudentsClassesToStudents.Select(studentClassToStudent =>
                studentClassToStudent.Student)
                .ToList();
    }
}
