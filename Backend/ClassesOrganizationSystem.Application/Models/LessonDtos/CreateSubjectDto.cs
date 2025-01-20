using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSystem.Application.Models.LessonDtos
{
    public class CreateSubjectDto
    {
        public string Title { get; set; } = null!;

        public Subject MapToSubject()
        {
            return new Subject
            {
                Title = Title,
            };
        }
    }
}
