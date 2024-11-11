using ClassesOrganizationSystem.Domain.Entities;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface ISchoolRepository
    {
        Task<School?> GetSchoolByIdAsync(int id);

        Task<IEnumerable<School>> ListSchoolsAsync(
            string? query, int pageNum = 1, int pageSize = 5);

        void AddSchool(School school);

        void RemoveSchool(School school);
    }
}
