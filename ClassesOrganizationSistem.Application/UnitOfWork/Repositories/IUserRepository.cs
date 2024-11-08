using ClassesOrganizationSistem.Domain.Entities;
using ClassesOrganizationSistem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Identity;

namespace ClassesOrganizationSistem.Application.UnitOfWork.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<IEnumerable<User>> ListUsersAsync(
            string? query, int pageNum = 1, int pageSize = 10);

        Task<IEnumerable<IdentityError>?> RegisterUserAsync(
            User user, string password);

        void RemoveUser(User user);

        void AddUserToClass(
            User user, StudentsClass studentsClass);

        void AddUserToSchool(
            User user, School school);

        Task RemoveUserFromClassAsync(
            User user, StudentsClass studentsClass);

        Task RemoveUserFromSchoolAsync(
            User user, School school);
    }
}
