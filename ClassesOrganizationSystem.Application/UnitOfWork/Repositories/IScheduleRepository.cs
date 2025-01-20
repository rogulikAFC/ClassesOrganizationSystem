using ClassesOrganizationSystem.Domain.Aggregates;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface IScheduleRepository
    {
        Task<Lesson?> GetLessonByIdAsync(int id);

        void RemoveLesson(Lesson lesson);

        void AddLesson(Lesson lesson);

        Task<LessonsSchedule?> GetLessonsScheduleByIdAsync(int id);

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

        Task<ListOfLessons> GetLessonsScheduleForRoomForDayOfWeekAsync(
            Room room, int dayOfWeek);

        Task<ListOfLessons> GetLessonsScheduleForRoomForDateAsync(
            Room room, DateOnly date);

        void RemoveLessonsSchedule(LessonsSchedule lessonsSchedule);

        void AddLessonsSchedule(LessonsSchedule lessonsSchedule);

        void AddSubject(Subject subject);

        void RemoveSubject(Subject subject);

        Task<Subject?> GetSubjectByIdAsync(int id);

        Task<IEnumerable<Subject>> ListSubjectsAsync(string? query, int pageNum, int pageSize);
    }
}
