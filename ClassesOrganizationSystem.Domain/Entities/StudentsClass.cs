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

        public IEnumerable<StudentsClassToStudent> StudentsClassesToStudents
            = new List<StudentsClassToStudent>();

        public IEnumerable<User> Students
            => StudentsClassesToStudents.Select(studentClassToStudent =>
                studentClassToStudent.Student);
    }
}
