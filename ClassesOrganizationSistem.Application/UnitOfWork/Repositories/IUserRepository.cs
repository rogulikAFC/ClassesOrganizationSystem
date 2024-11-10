using ClassesOrganizationSistem.Domain.Entities;
using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSistem.Application.UnitOfWork.Repositories
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

        void AddUserToSchool(
            User user, School school, SchoolRole role);

        Task RemoveUserFromClassAsync(
            User user, StudentsClass studentsClass);

        Task RemoveUserFromSchoolAsync(
            User user, School school, SchoolRole role);

        void AcceptAddRoleRequest(AddRoleRequest request);

        void DenyAddRoleRequest(AddRoleRequest request);

        void AddRoleToUserInSchool(User user, School school, SchoolRole role);

        void RemoveRoleFromUserInSchool(User user, School school, SchoolRole role);

        Task<SchoolRole?> GetRoleByIdAsync(int id);

        Task<IEnumerable<SchoolRole>> GetRolesByQuery(string query);
    }
}
