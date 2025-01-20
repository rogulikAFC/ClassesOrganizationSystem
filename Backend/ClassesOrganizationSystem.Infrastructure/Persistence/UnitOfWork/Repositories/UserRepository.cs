using ClassesOrganizationSystem.Application.UnitOfWork.Repositories;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClassesOrganizationSystem.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClassesOrganizationSystemDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(
            ClassesOrganizationSystemDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void AcceptAddRoleRequest(AddRoleRequest request)
        {
            AddUserWithRoleToSchool(request.User, request.School, request.Role);

            _context.Remove(request);
        }

        public void AddAddRoleRequest(AddRoleRequest request)
        {
            _context.Add(request);
        }

        public async Task AddUserToClassAsync(
            User user, StudentsClass studentsClass, School? school)
        {
            var role = (await ListSchoolRolesByQueryAsync("student"))
                .FirstOrDefault();

            if (role == null)
            {
                return;
            }

            if (school == null)
            {
                AddUserWithRoleToSchool(user, studentsClass.School, role);
            } 
            else 
            {
                AddUserWithRoleToSchool(user, school, role);
            }

            var userToClass = new StudentsClassToStudent
            {
                Student = user,
                StudentsClass = studentsClass
            };

            _context.Add(userToClass);
        }

        public void AddUserWithRoleToSchool(User user, School school, SchoolRole role)
        {
            var isUserAlreadyHasRole = _context.UsersRolesInSchools
                .Any(usersRoleInSchool =>
                    usersRoleInSchool.UserId == user.Id
                    && usersRoleInSchool.SchoolId == school.Id
                    && usersRoleInSchool.SchoolRoleId == role.Id);

            if (isUserAlreadyHasRole)
            {
                return;
            }

            var userRoleInSchool = new UserRoleInSchool
            {
                User = user,
                School = school,
                SchoolRole = role
            };

            _context.Add(userRoleInSchool);
        }

        public void DenyAddRoleRequest(AddRoleRequest request)
        {
            _context.Remove(request);
        }

        public async Task<SchoolRole?> GetSchoolRoleByIdAsync(int id)
        {
            return await _context.SchoolRoles
                .FirstOrDefaultAsync(schoolRole => schoolRole.Id == id);
        }

        public async Task<IEnumerable<SchoolRole>> ListSchoolRolesByQueryAsync(string? query)
        {
            return await _context.SchoolRoles
                .Where(schoolRole =>
                    query == null || schoolRole.Name.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users

                .Include(user => user.SchoolRoles)
                .ThenInclude(roleInSchool => roleInSchool.SchoolRole)

                .Include(user => user.SchoolRoles)
                .ThenInclude(roleInSchool => roleInSchool.School)

                .Include(user => user.RolesToUser)
                .ThenInclude(roleToUser => roleToUser.Role)

                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users

                .Include(user => user.SchoolRoles)
                .ThenInclude(roleInSchool => roleInSchool.SchoolRole)

                .Include(user => user.SchoolRoles)
                .ThenInclude(roleInSchool => roleInSchool.School)

                .Include(user => user.RolesToUser)
                .ThenInclude(roleToUser => roleToUser.Role)

                .FirstOrDefaultAsync(user =>
                    user.UserName.Equals(username, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<IEnumerable<User>> ListUsersAsync(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            return await _context.Users

                .Where(user =>
                    query == null ||
                        user.UserName.ToLower().Contains(query.ToLower())
                        || user.Name.ToLower().Contains(query.ToLower())
                        || user.Surname.ToLower().Contains(query.ToLower()))

                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<IdentityError>?> RegisterUserAsync(User user, string password)
        {
            var registrationResult = await _userManager.CreateAsync(user, password);

            if (!registrationResult.Succeeded)
            {
                return registrationResult.Errors;
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "user");

            if (!addToRoleResult.Succeeded)
            {
                return addToRoleResult.Errors;
            }

            return null;
        }

        public async Task RemoveRoleFromUserInSchoolAsync(User user, School school, SchoolRole role)
        {
            var userRoleInSchool = await _context.UsersRolesInSchools

                .FirstOrDefaultAsync(userRoleInSchool =>
                    userRoleInSchool.SchoolRoleId == role.Id
                    && userRoleInSchool.UserId == user.Id
                    && userRoleInSchool.SchoolId == school.Id);

            if (userRoleInSchool == null)
            {
                return;
            }

            _context.Remove(userRoleInSchool);
        }

        public void RemoveUser(User user)
        {
            _context.Remove(user);
        }

        public async Task RemoveUserFromClassAsync(User user, StudentsClass studentsClass)
        {
            var classToUser = await _context.StudentsClassesToStudents
                .FirstOrDefaultAsync(classToStudent =>
                    classToStudent.StudentsClass == studentsClass
                    && classToStudent.Student == user);

            if (classToUser == null)
            {
                throw new Exception("Пользователь не в школе");
            }
            _context.Remove(classToUser);
        }

        public void RemoveUserFromSchool(User user, School school)
        {
            var classesToUser = _context.StudentsClassesToStudents
                .Where(studentsClassesToUser =>
                    studentsClassesToUser.Student == user
                    && studentsClassesToUser.StudentsClass.School == school);

            _context.RemoveRange(classesToUser);

            var rolesInSchool = _context.UsersRolesInSchools
                .Where(roleInSchool => 
                    roleInSchool.School == school
                    && roleInSchool.User == user);

            _context.RemoveRange(rolesInSchool);
        }

        public async Task<IEnumerable<AddRoleRequest>> ListAddRoleRequestsOfSchool(School school, int pageNum = 1, int pageSize = 10)
        {
            return await _context.AddRoleRequests

                .Include(request => request.User)
                .Include(request => request.Role)

                .Where(addRoleRequest => addRoleRequest.SchoolId == school.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public void AddClass(StudentsClass studentsClass)
        {
            _context.Add(studentsClass);
        }

        public void RemoveClass(StudentsClass studentsClass)
        {
            _context.Remove(studentsClass);
        }

        public async Task<IEnumerable<StudentsClass>> ListClassesOfSchool(
            School school, string? query, int pageSize = 10, int pageNum = 1)
        {
            return await _context.StudentsClasses

                .Include(studentsClass => studentsClass.StudentsClassesToStudents)

                .Where(studentsClass => 
                    studentsClass.SchoolId == school.Id
                    && (query == null || studentsClass.Title.ToLower().Contains(query.ToLower())))

                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)

                .ToListAsync();
        }

        public async Task<StudentsClass?> GetClassByIdAsync(int id)
        {
            return await _context.StudentsClasses

                .Include(studentsClass => studentsClass.StudentsClassesToStudents)
                .ThenInclude(classesToStudents => classesToStudents.Student)

                .Include(studentsClass => studentsClass.School)

                .FirstOrDefaultAsync(studentsClass => studentsClass.Id == id);
        }

        public async Task<AddRoleRequest?> GetAddRoleRequestByIdAsync(int id)
        {
            return await _context.AddRoleRequests

                .Include(request => request.School)
                .Include(request => request.User)
                .Include(request => request.Role)

                .FirstOrDefaultAsync(request => request.Id == id);
        }
    }
}
