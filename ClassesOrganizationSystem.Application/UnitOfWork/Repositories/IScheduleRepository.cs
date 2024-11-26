using ClassesOrganizationSystem.Domain.Aggregates;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface IScheduleRepository
    {
        Task<Lesson?> GetLessonById(int id);

        Task<LessonsSchedule?> GetLessonsScheduleByIdAsync(int id);

        void RemoveLesson(Lesson lesson);

        void AddLesson(Lesson lesson);

        Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDayOfWeekAsync(
            StudentsClass studentsClass, int dayOfWeek);

        Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDateAsync(
            StudentsClass studentsClass, DateOnly date);

        Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            StudentsClass studentsClass, DateOnly firstWeekDay);

        Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            StudentsClass studentsClass);

        Task<ListOfLessons> GetLessonsScheduleForTeacherForDateAsync(
            User teacher, DateOnly date);

        Task<ListOfLessons> GetLessonsScheduleForTeacherByDayOfWeekAsync(
            User teacher, int dayOfWeek);

        Task<IEnumerable<ListOfLessons>> GetLessonsSchedulesForTeacherForWeekAsync(
            User teacher, DateOnly firstWeekDay);

        Task<IEnumerable<ListOfLessons>> GetLessonsSchedulesForTeacherForWeekAsync(
            User teacher);

        void RemoveLessonsSchedule(LessonsSchedule lessonsSchedule);

        void AddLessonsSchedule(LessonsSchedule lessonsSchedule);

        Task<ListOfLessons> GetLessonsScheduleForRoomForDayOfWeekAsync(
            Room room, int dayOfWeek);

        Task<ListOfLessons> GetLessonsScheduleForRoomForDate(
            Room room, DateOnly date);
    }
}
