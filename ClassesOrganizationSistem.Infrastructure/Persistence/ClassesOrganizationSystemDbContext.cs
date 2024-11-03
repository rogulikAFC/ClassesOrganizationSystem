using ClassesOrganizationSistem.Domain.Entities;
using ClassesOrganizationSistem.Domain.Entities.RoomEntities;
using ClassesOrganizationSistem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSistem.Infrastructure.Persistence
{
    public class ClassesOrganizationSystemDbContext : IdentityDbContext<User, Role, int>
    {
        public ClassesOrganizationSystemDbContext(
            DbContextOptions<ClassesOrganizationSystemDbContext> options)
            : base(options) { }

        public DbSet<Lesson> Lessons { get; set; } = null!;

        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<LessonsSchedule> LessonsSchedules { get; set; } = null!;

        public DbSet<LessonToLessonsSchedule> LessonsToLessonsSchedules { get; set; } = null!;

        public DbSet<Equipment> Equipments { get; set; } = null!;

        public DbSet<Room> Rooms { get; set; } = null!;

        public DbSet<RoomToEquipment> RoomsToEquipments { get; set; } = null!;

        public DbSet<RoomStatus> RoomStatuses { get; set; } = null!;

        public DbSet<School> Schools { get; set; } = null!;

        public DbSet<StudentsClassToStudent> StudentsClassesToStudents { get; set; } = null!;
    }
}
