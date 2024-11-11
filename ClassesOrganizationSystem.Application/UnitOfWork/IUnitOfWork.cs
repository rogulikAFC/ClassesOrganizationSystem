using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;

namespace ClassesOrganizationSystem.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        ISchoolRepository SchoolRepository { get; }

        IScheduleRepository ScheduleRepository { get; }

        IRoomRepository RoomRepository { get; }

        IUserRepository UserRepository { get; }

        Task SaveChangesAsync();
    }
}
