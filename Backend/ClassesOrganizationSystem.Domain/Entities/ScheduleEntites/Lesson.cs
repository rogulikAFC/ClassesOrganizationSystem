using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSystem.Domain.Entities.ScheduleEntites
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SerialNumber { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public User Teacher { get; set; } = null!;

        [Required]
        public int RoomId { get; set; }

        public Room Room { get; set; } = null!;

        [Required]
        public int SubjectId { get; set; }

        public Subject Subject { get; set; } = null!;

        [Required]
        public int LessonsScheduleId { get; set; }

        public LessonsSchedule LessonsSchedule { get; set; } = null!;
    }
}
