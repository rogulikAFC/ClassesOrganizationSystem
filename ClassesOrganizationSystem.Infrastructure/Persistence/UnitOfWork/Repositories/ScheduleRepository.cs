using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Aggregates;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<Lesson?> GetLessonById(int id)
        {
            return await _context.Lessons
                .FirstOrDefaultAsync(lesson => lesson.Id == id);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDateAsync(
            StudentsClass studentsClass, DateOnly date)
        {
            return await _context.LessonsSchedules
                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.StudentsClass == studentsClass
                    && lessonsSchedule.Date == date);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByClassIdAndDayOfWeekAsync(
            StudentsClass studentsClass, int dayOfWeek)
        {
            return await _context.LessonsSchedules
                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.StudentsClass == studentsClass
                    && lessonsSchedule.DayOfWeek == dayOfWeek);
        }

        public async Task<LessonsSchedule?> GetLessonsScheduleByIdAsync(int id)
        {
            return await _context.LessonsSchedules
                .FirstOrDefaultAsync(lessonsSchedule =>
                    lessonsSchedule.Id == id);
        }

        public async Task<ListOfLessons> GetLessonsScheduleForRoomForDate(Room room, DateOnly date)
        {
            var listOfLessons = new ListOfLessons
            {
                Date = date,
            };

            listOfLessons.Lessons = await _context.Lessons

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

            return await _context.Lessons

                .Where(lesson =>
                    lesson.Teacher == teacher
                    && lesson.LessonsSchedule.Date >= firstWeekDay
                    && lesson.LessonsSchedule.Date <= lastWeekDay)

                .GroupBy(lesson => lesson.LessonsSchedule.Date)
                .Select(lessonsGroup => new ListOfLessons
                {
                    Date = lessonsGroup.First().LessonsSchedule.Date,
                    Lessons = lessonsGroup
                })
                .OrderBy(lessonsByDate => lessonsByDate.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<ListOfLessons>> GetLessonsSchedulesForTeacherForWeekAsync(
            User teacher)
        {
            return await _context.Lessons
                .Where(lesson =>
                    lesson.Teacher == teacher)
                .GroupBy(lesson => lesson.LessonsSchedule.DayOfWeek)
                .Select(lessonsGroup => new ListOfLessons
                {
                    DayOfWeek = lessonsGroup.First().LessonsSchedule.DayOfWeek,
                    Lessons = lessonsGroup
                })
                .OrderBy(listOfLessons => listOfLessons.DayOfWeek)
                .ToListAsync();
        }

        public async Task<IEnumerable<LessonsSchedule>> GetLessonsSchedulesForWeekForClassAsync(
            StudentsClass studentsClass, DateOnly firstWeekDay)
        {
            var lastWeekDay = firstWeekDay.AddDays(7);

            return await _context.LessonsSchedules

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

        public void RemoveLesson(Lesson lesson)
        {
            _context.Remove(lesson);
        }

        public void RemoveLessonsSchedule(LessonsSchedule lessonsSchedule)
        {
            _context.Remove(lessonsSchedule);
        }
    }
}
