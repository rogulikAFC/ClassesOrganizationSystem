using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface IScheduleRepository
    {
        Task<Lesson?> GetLessonById(int id);

        Task<LessonsSchedule?> GetLessonsScheduleByIdAsync(int id);

        void RemoveLesson(Lesson lesson);

        void AddLesson(Lesson lesson);

        Task<LessonsSchedule> GetLessonsScheduleByClassIdAndDayOfWeekAsync(
            int classId, int dayOfWeek);

        Task<LessonsSchedule> GetLessonsScheduleByClassIdAndDateAsync(
            int classId, DateOnly? date);

        Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            int classId, DateOnly firstWeekDay);

        Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            int classId);

        Task<LessonsSchedule> GetLessonsScheduleForTeacherForDateAsync(
            int teacherId, DateOnly date);

        Task<LessonsSchedule> GetLessonsScheduleForTeacherByDayOfWeekAsync(
            int teacherId, int dayOfWeek);

        Task<LessonsSchedule> GetLessonsSchedulesForTeacherForWeekAsync(
            int teacherId, DateOnly firstWeekDay);

        Task<LessonsSchedule> GetLessonsSchedulesForTeacherForWeekAsync(
            int teacherId);

        void RemoveLessonsSchedule(LessonsSchedule lessonsSchedule);

        void AddLessonsSchedule(LessonsSchedule lessonsSchedule);

        Task<LessonsSchedule> GetLessonsScheduleForRoomForDayOfWeek(
            int roomId, int dayOfWeek);

        Task<LessonsSchedule> GetLessonsScheduleForRoomForDate(
            int roomId, DateOnly date);
    }
}
