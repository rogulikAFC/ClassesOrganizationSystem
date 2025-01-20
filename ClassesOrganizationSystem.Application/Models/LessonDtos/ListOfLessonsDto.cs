using ClassesOrganizationSystem.Domain.Aggregates;

namespace ClassesOrganizationSystem.Application.Models.LessonDtos
{
    public class ListOfLessonsDto
    {
        public DateOnly? Date { get; set; }

        public int? DayOfWeek { get; set; }

        public IEnumerable<LessonDto> Lessons { get; set; }
            = new List<LessonDto>();

        public static ListOfLessonsDto MapFromListOfLessons(
            ListOfLessons listOfLessons)
        {
            return new ListOfLessonsDto
            {
                Date = listOfLessons.Date,
                DayOfWeek = listOfLessons.DayOfWeek,

                Lessons = listOfLessons.Lessons
                    .Select(LessonDto.MapFromLesson)
            };
        }
    }
}
