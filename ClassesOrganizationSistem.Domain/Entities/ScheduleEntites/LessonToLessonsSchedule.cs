using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClassesOrganizationSistem.Domain.Entities.ScheduleEntites
{
    [PrimaryKey(nameof(LessonId), nameof(LessonsScheduleId))]
    public class LessonToLessonsSchedule
    {
        [Required]
        public int LessonId { get; set; }

        [Required]
        public int LessonsScheduleId { get; set; }

        public Lesson Lesson { get; set; } = null!;

        public LessonsSchedule LessonsSchedule { get; set; } = null!;
    }
}
