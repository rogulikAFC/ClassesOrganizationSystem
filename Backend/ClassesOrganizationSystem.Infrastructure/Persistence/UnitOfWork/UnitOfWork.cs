using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClassesOrganizationSystemDbContext _context;
        private readonly UserManager<User> _userManager; 

        public UnitOfWork(
            ClassesOrganizationSystemDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ISchoolRepository SchoolRepository => new SchoolRepository(_context);

        public IScheduleRepository ScheduleRepository => new ScheduleRepository(_context);

        public IRoomRepository RoomRepository => new RoomRepository(_context);

        public IUserRepository UserRepository => new UserRepository(_context, _userManager);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
