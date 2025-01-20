using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Aggregates;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ClassesOrganizationSystemDbContext _context;

        public ScheduleRepository(ClassesOrganizationSystemDbContext context)
        {
            _context = context;
        }

        public void AddLesson(Lesson lesson)
        {
            _context.Add(lesson);
        }

        public void AddLessonsSchedule(LessonsSchedule lessonsSchedule)
        {
            _context.Add(lessonsSchedule);
        }

        public void AddSubject(Subject subject)
        {
            _context.Add(subject);
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons

                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Room)
                .Include(lesson => lesson.Subject)

                .FirstOrDefaultAsync(lesson => lesson.Id == id);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDateAsync(
            StudentsClass studentsClass, DateOnly date)
        {
            return await _context.LessonsSchedules

                .Include(schedule => schedule.StudentsClass)
                .ThenInclude(StudentsClass => StudentsClass.StudentsClassesToStudents)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Teacher)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Room)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Subject)

                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.StudentsClass == studentsClass
                    && lessonsSchedule.Date == date);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDayOfWeekAsync(
            StudentsClass studentsClass, int dayOfWeek)
        {
            return await _context.LessonsSchedules

                .Include(schedule => schedule.StudentsClass)
                .ThenInclude(StudentsClass => StudentsClass.StudentsClassesToStudents)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Teacher)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Room)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Subject)

                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.StudentsClass == studentsClass
                    && lessonsSchedule.DayOfWeek == dayOfWeek);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByIdAsync(int id)
        {
            return await _context.LessonsSchedules

                .Include(schedule => schedule.StudentsClass)
                .ThenInclude(StudentsClass => StudentsClass.StudentsClassesToStudents)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Teacher)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Room)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Subject)

                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.Id == id);
        }

        public async Task<ListOfLessons> GetLessonsScheduleForRoomForDateAsync(Room room, DateOnly date)
        {
            var listOfLessons = new ListOfLessons
            {
                Date = date,
            };

            listOfLessons.Lessons = await _context.Lessons

                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Room)
                .Include(lesson => lesson.Subject)

                .Where(lesson =>
                    lesson.Room == room
                    && lesson.LessonsSchedule.Date == date)

                .OrderBy(lesson => lesson.SerialNumber)
                .ToListAsync();

            return listOfLessons;
        }

        public async Task<ListOfLessons> GetLessonsScheduleForRoomForDayOfWeekAsync(
            Room room, int dayOfWeek)
        {
            var listOfLessons = new ListOfLessons
            {
                DayOfWeek = dayOfWeek,
            };

            listOfLessons.Lessons = await _context.Lessons

                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Room)
                .Include(lesson => lesson.Subject)

                .Where(lesson =>
                    lesson.Room == room
                    && lesson.LessonsSchedule.DayOfWeek == dayOfWeek)

                .OrderBy(lesson => lesson.SerialNumber)
                .ToListAsync();

            return listOfLessons;
        }

        public async Task<ListOfLessons> GetLessonsScheduleForTeacherByDayOfWeekAsync(
            User teacher, int dayOfWeek)
        {
            var listOfLessons = new ListOfLessons
            {
                DayOfWeek = dayOfWeek,
            };

            listOfLessons.Lessons = await _context.Lessons

                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Room)
                .Include(lesson => lesson.Subject)

                .Where(lesson =>
                    lesson.Teacher == teacher
                    && lesson.LessonsSchedule.DayOfWeek == dayOfWeek)

                .OrderBy(lesson => lesson.SerialNumber)
                .ToListAsync();

            return listOfLessons;
        }

        public async Task<ListOfLessons> GetLessonsScheduleForTeacherForDateAsync(
            User teacher, DateOnly date)
        {
            var listOfLessons = new ListOfLessons
            {
                Date = date,
            };

            listOfLessons.Lessons = await _context.Lessons
                
                .Include(lesson => lesson.Teacher)
                .Include(lesson => lesson.Room)
                .Include(lesson => lesson.Subject)

                .Where(lesson =>
                    lesson.Teacher == teacher
                    && lesson.LessonsSchedule.Date == date)

                .OrderBy(lesson => lesson.SerialNumber)
                .ToListAsync();

            return listOfLessons;
        }

        public async Task<IEnumerable<ListOfLessons>> GetLessonsSchedulesForTeacherForWeekAsync(
            User teacher, DateOnly firstWeekDay)
        {
            var lastWeekDay = firstWeekDay.AddDays(7);

            var listsOfLessons = new List<ListOfLessons>();

            var dateRange = Enumerable.Range(1, 7)
                .Select(firstWeekDay.AddDays)
                .ToList();

            foreach (DateOnly date in dateRange)
            {
                var lessons = await GetLessonsScheduleForTeacherForDateAsync(
                    teacher, date);

                listsOfLessons.Add(lessons);
            }

            return listsOfLessons;
        }

        public async Task<IEnumerable<ListOfLessons>> GetLessonsSchedulesForTeacherForWeekAsync(
            User teacher)
        {
            var listsOfLessons = new List<ListOfLessons>();

            foreach (int i in Enumerable.Range(1, 7))
            {
                var lessons = await GetLessonsScheduleForTeacherByDayOfWeekAsync(
                    teacher, i);

                listsOfLessons.Add(lessons);
            }

            return listsOfLessons;
        }

        public async Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            StudentsClass studentsClass, DateOnly firstWeekDay)
        {
            var lastWeekDay = firstWeekDay.AddDays(7);

            return await _context.LessonsSchedules

                .Include(schedule => schedule.StudentsClass)
                .ThenInclude(StudentsClass => StudentsClass.StudentsClassesToStudents)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Teacher)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Room)

                .Include(schedule => schedule.Lessons)
                .ThenInclude(lesson => lesson.Subject)

                .Where(lessonsSchedule => 
                    lessonsSchedule.StudentsClass == studentsClass
                    && lessonsSchedule.Date >= firstWeekDay
                    && lessonsSchedule.Date <= lastWeekDay)

                .OrderBy(lessonsSchedule => lessonsSchedule.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            StudentsClass studentsClass)
        {
            return await _context.LessonsSchedules

                .Where(lessonsSchedule =>
                    lessonsSchedule.StudentsClass == studentsClass)
                
                .OrderBy(lessonsSchedule => lessonsSchedule.DayOfWeek)
                .ToListAsync();
        }

        public async Task<Subject?> GetSubjectByIdAsync(int id)
        {
            return await _context.Subjects
                .FirstOrDefaultAsync(subject => subject.Id == id);
        }

        public async Task<IEnumerable<Subject>> ListSubjectsAsync(string? query, int pageNum = 1, int pageSize = 10)
        {
            return await _context.Subjects
                .Where(subject => 
                    query == null || subject.Title.ToLower().Contains(query.ToLower()))
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public void RemoveLesson(Lesson lesson)
        {
            _context.Remove(lesson);
        }

        public void RemoveLessonsSchedule(LessonsSchedule lessonsSchedule)
        {
            _context.Remove(lessonsSchedule);
        }

        public void RemoveSubject(Subject subject)
        {
            _context.Remove(subject);
        }
    }
}
