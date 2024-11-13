using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClassesOrganizationSystemDbContext _context;

        public UnitOfWork(ClassesOrganizationSystemDbContext context)
        {
            _context = context;
        }

        public ISchoolRepository SchoolRepository => throw new NotImplementedException();

        public IScheduleRepository ScheduleRepository => throw new NotImplementedException();

        public IRoomRepository RoomRepository => new RoomRepository(_context);

        public IUserRepository UserRepository => throw new NotImplementedException();

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
