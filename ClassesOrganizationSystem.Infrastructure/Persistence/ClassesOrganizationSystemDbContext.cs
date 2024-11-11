using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSystem.Infrastructure.Persistence
{
    public class ClassesOrganizationSystemDbContext : IdentityDbContext<User, Role, int>
    {
        public ClassesOrganizationSystemDbContext(
            DbContextOptions<ClassesOrganizationSystemDbContext> options)
            : base(options) { }

        public DbSet<Lesson> Lessons { get; set; } = null!;

        public DbSet<Subject> Subjects { get; set; } = null!;

        public DbSet<LessonsSchedule> LessonsSchedules { get; set; } = null!;

        public DbSet<Equipment> Equipments { get; set; } = null!;

        public DbSet<Room> Rooms { get; set; } = null!;

        public DbSet<RoomToEquipment> RoomsToEquipments { get; set; } = null!;

        public DbSet<RoomStatus> RoomStatuses { get; set; } = null!;

        public DbSet<School> Schools { get; set; } = null!;

        public DbSet<StudentsClassToStudent> StudentsClassesToStudents { get; set; } = null!;

        public DbSet<UserToSchool> UsersToSchools { get; set; } = null!;

        public DbSet<SchoolRole> SchoolRoles { get; set; } = null!;

        public DbSet<UserRoleInSchool> UsersRolesInSchools { get; set; } = null!;

        public DbSet<AddRoleRequest> AddRoleRequests { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(user => user.Schools)
                .WithMany(school => school.Users)
                .UsingEntity<UserToSchool>();

            builder.Entity<StudentsClass>()
                .HasMany(studentsClass => studentsClass.Students)
                .WithMany(student => student.StudentsClasses)
                .UsingEntity<StudentsClassToStudent>();
        }
    }
}
