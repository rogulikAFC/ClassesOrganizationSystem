using ClassesOrganizationSistem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSistem.Application.Models.LessonDtos
{
    public class SubjectDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public static SubjectDto MapFromSubject(Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Title = subject.Title,
            };
        }
    }
}
