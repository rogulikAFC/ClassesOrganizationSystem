using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private readonly ClassesOrganizationSystemDbContext _context;

        public SchoolRepository(ClassesOrganizationSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddSchool(School school, User creator)
        {
            _context.Add(school);

            var schoolRole = await _context.SchoolRoles
                .FirstOrDefaultAsync(schoolRole => schoolRole.Name == "admin");

            if (schoolRole == null)
            {
                throw new Exception("School role not found");
            }

            var userRoleInSchool = new UserRoleInSchool()
            {
                School = school,
                User = creator,
                SchoolRole = schoolRole
            };

            _context.Add(userRoleInSchool);
        }

        public async Task<School?> GetSchoolByIdAsync(int id)
        {
            return await _context.Schools
                .FirstOrDefaultAsync(school => school.Id == id);
        }

        public async Task<IEnumerable<School>> ListSchoolsAsync(
            string? query, int pageNum = 1, int pageSize = 5)
        {
            return await _context.Schools

                .Where(school =>
                    query == null
                        || school.Title.Contains(query, StringComparison.CurrentCultureIgnoreCase)
                        || (school.Address != null
                            && school.Address.Contains(query, StringComparison.CurrentCultureIgnoreCase)))

                .ToListAsync();
        }

        public void RemoveSchool(School school)
        {
            _context.Remove(school);
        }
    }
}
