using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface ISchoolRepository
    {
        Task<School?> GetSchoolByIdAsync(int id);

        Task<IEnumerable<School>> ListSchoolsAsync(
            string? query, int pageNum = 1, int pageSize = 5);

        Task AddSchool(School school, User creator);

        void RemoveSchool(School school);
    }
}
