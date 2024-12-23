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

        Task AddUserToClassAsync(
            User user, StudentsClass studentsClass, School? school);

        Task RemoveUserFromClassAsync(
            User user, StudentsClass studentsClass);

        void AddClass(StudentsClass studentsClass);

        void RemoveClass(StudentsClass studentsClass);

        Task<IEnumerable<StudentsClass>> ListClassesOfSchool(
            School school, string? query, int pageSize = 10, int pageNum = 1);

        Task<StudentsClass?> GetClassByIdAsync(int id);

        void RemoveUserFromSchool(
            User user, School school);

        Task<IEnumerable<AddRoleRequest>> ListAddRoleRequestsOfSchool(
            School school, int pageNum = 1, int pageSize = 10);

        void AddAddRoleRequest(AddRoleRequest request);

        void AcceptAddRoleRequest(AddRoleRequest request);

        void DenyAddRoleRequest(AddRoleRequest request);

        Task<AddRoleRequest?> GetAddRoleRequestByIdAsync(int id);

        Task RemoveRoleFromUserInSchoolAsync(User user, School school, SchoolRole role);

        Task<SchoolRole?> GetSchoolRoleByIdAsync(int id);

        Task<IEnumerable<SchoolRole>> ListSchoolRolesByQueryAsync(string? query);

        void AddUserWithRoleToSchool(User user, School school, SchoolRole role);
    }
}
