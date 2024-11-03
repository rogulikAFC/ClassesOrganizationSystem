﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassesOrganizationSistem.Domain.Entities.ScheduleEntites
{
    public class LessonsSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentsClassId { get; set; }

        public StudentsClass StudentsClass { get; set; } = null!;

        [Range(0, 6)]
        public int? DayOfWeek { get; set; }

        public DateOnly? Date { get; set; }


        public IEnumerable<LessonToLessonsSchedule> LessonsToLessonsSchedules
            = new List<LessonToLessonsSchedule>();

        public IEnumerable<Lesson> Lessons
            => LessonsToLessonsSchedules.Select(lessonToLessonsSchedule =>
                lessonToLessonsSchedule.Lesson);
    }
}
