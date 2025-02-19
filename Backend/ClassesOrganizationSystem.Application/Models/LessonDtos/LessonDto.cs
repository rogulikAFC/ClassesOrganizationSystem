using ClassesOrganizationSystem.Application.Models.RoomDtos;
using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSystem.Application.Models.LessonDtos
{
    public class LessonDto
    {
        public int Id { get; set; }

        public int SerialNumber { get; set; }
        
        public UserAnnotationDto Teacher { get; set; } = null!;

        public RoomAnnotationDto Room { get; set; } = null!;

        public SubjectDto Subject { get; set; } = null!;

        public StudentsClassAnnotationDto StudentsClass { get; set; } = null!;

        public static LessonDto MapFromLesson(Lesson lesson)
        {
            return new LessonDto
            {
                Id = lesson.Id,
                SerialNumber = lesson.SerialNumber,
                Teacher = UserAnnotationDto.MapFromUser(lesson.Teacher),
                Room = RoomAnnotationDto.MapFromRoom(lesson.Room),
                Subject = SubjectDto.MapFromSubject(lesson.Subject),
                StudentsClass = StudentsClassAnnotationDto.MapFromStudentsClass(
                    lesson.LessonsSchedule.StudentsClass)
            };
        }
    }
}
