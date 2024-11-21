using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSystem.Domain.Aggregates
{
    public class ListOfLessons
    {
        public DateOnly? Date { get; set; }

        public int? DayOfWeek { get; set; }

        public IEnumerable<Lesson> Lessons { get; set; } 
            = new List<Lesson>();
    }
}
