using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSystem.Application.UnitOfWork.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<User?> GetUserByUsernameAsync(
            string username);

        Task<IEnumerable<User>> ListUsersAsync(
            string? query, int pageNum = 1, int pageSize = 10);

        Task<IEnumerable<IdentityError>?> RegisterUserAsync(
            User user, string password);

        void RemoveUser(User user);

        void AddUserToClass(
            User user, StudentsClass studentsClass);

        Task RemoveUserFromClassAsync(
            User user, StudentsClass studentsClass);

        void RemoveUserFromSchool(
            User user, School school);

        void AddAddRoleRequest(AddRoleRequest request);

        void AcceptAddRoleRequest(AddRoleRequest request);

        void DenyAddRoleRequest(AddRoleRequest request);

        void RemoveRoleFromUserInSchool(User user, School school, SchoolRole role);

        Task<SchoolRole?> GetSchoolRoleByIdAsync(int id);

        Task<IEnumerable<SchoolRole>> GetSchoolRolesByQuery(string query);

        void AddUserWithRoleToSchool(User user, School school, SchoolRole role);
    }
}
