using ClassesOrganizationSistem.Domain.Entities;
using ClassesOrganizationSistem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSistem.Application.Models.LessonDtos
{
    public class LessonsScheduleDto
    {
        public int Id { get; set; }

        public StudentsClass StudentsClass { get; set; } = null!;

        public int? DayOfWeek { get; set; }

        public DateOnly? Date { get; set; }

        public IEnumerable<LessonDto> Lessons { get; set; } 
            = new List<LessonDto>();

        public static LessonsScheduleDto MapFromLessonsSchedule(
            LessonsSchedule lessonsSchedule)
        {
            return new LessonsScheduleDto
            {
                Id = lessonsSchedule.Id,
                StudentsClass = lessonsSchedule.StudentsClass,
                DayOfWeek = lessonsSchedule.DayOfWeek,
                Date = lessonsSchedule.Date,
                Lessons = lessonsSchedule.Lessons
                    .Select(LessonDto.MapFromLesson)
            };
        }
    }
}
