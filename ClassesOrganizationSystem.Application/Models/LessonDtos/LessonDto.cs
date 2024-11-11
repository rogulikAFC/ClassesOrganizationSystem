using ClassesOrganizationSystem.Application.Models.RoomDtos;
using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSystem.Application.Models.LessonDtos
{
    public class LessonDto
    {
        public int Id { get; set; }

        public int SerialNumber { get; set; }

        public UserDto Teacher { get; set; } = null!;

        public RoomDto Room { get; set; } = null!;

        public SubjectDto Subject { get; set; } = null!;

        public static LessonDto MapFromLesson(Lesson lesson)
        {
            return new LessonDto
            {
                Id = lesson.Id,
                SerialNumber = lesson.SerialNumber,
                Teacher = UserDto.MapFromUser(lesson.Teacher),
                Room = RoomDto.MapFromRoom(lesson.Room),
                Subject = SubjectDto.MapFromSubject(lesson.Subject)
            };
        }
    }
}
