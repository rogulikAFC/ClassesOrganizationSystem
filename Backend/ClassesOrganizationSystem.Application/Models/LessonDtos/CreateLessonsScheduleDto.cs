using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSystem.Application.Models.LessonDtos
{
    public class CreateLessonsScheduleDto
    {
        [Required]
        public int StudentsClassId { get; set; }

        [Range(0, 6)]
        public int? DayOfWeek { get; set; }

        public DateOnly? Date { get; set; }

        public LessonsSchedule MapToLessonsSchedule()
        {
            return new LessonsSchedule
            {
                StudentsClassId = StudentsClassId,
                DayOfWeek = DayOfWeek,
                Date = Date,
            };
        }
    }
}
