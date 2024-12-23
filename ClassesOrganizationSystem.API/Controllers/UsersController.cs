using ClassesOrganizationSystem.Application.Models.StudentsClassDtos;
using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUnitOfWork unitOfWork, ILogger<UsersController> logger,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto?>> RegisterUser(CreateUserDto createUserDto)
        {
            var user = createUserDto.MapToUser();

            var errors = await _unitOfWork.UserRepository
                .RegisterUserAsync(user, createUserDto.Password);
            
            if (errors != null)
            {
                return BadRequest(errors);
            }

            _logger.LogInformation(user.Name);

            await _unitOfWork.SaveChangesAsync();

            var userDto = UserDto.MapFromUser(user);

            return CreatedAtAction(nameof(RegisterUser), new
                {
                    user.Id

                }, userDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto?>> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(nameof(user));
            }

            var userDto = UserDto.MapFromUser(user);

            return Ok(userDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAnnotationDto>>> ListUsers(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            var users = await _unitOfWork.UserRepository
                .ListUsersAsync(query, pageNum, pageSize);

            var userDtos = new List<UserAnnotationDto>();

            foreach (var user in users)
            {
                var userDto = UserAnnotationDto.MapFromUser(user);

                userDtos.Add(userDto);
            }

            return Ok(userDtos);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var nameIdentitier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentitier == null)
            {
                return BadRequest(nameof(nameIdentitier));
            }

            var userId = int.Parse(nameIdentitier);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (user.Id != id)
            {
                return Unauthorized("Только пользователь может удалить свой аккаунт");
            }

            _unitOfWork.UserRepository.RemoveUser(user);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateUser(int id, JsonPatchDocument patchDocument)
        {
            var nameIdentitier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (nameIdentitier == null)
            {
                return BadRequest(nameof(nameIdentitier));
            }

            var userId = int.Parse(nameIdentitier);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (user.Id != id)
            {
                return Unauthorized("Только пользователь может изменять свои данные");
            }

            patchDocument.ApplyTo(user);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("School_Roles")]
        public async Task<ActionResult<SchoolRoleDto>> ListSchoolRoles(string? query)
        {
            var schoolRoles = await _unitOfWork.UserRepository
                .ListSchoolRolesByQueryAsync(query);

            var schoolRoleDtos = new List<SchoolRoleDto>();

            foreach (var schoolRole in schoolRoles)
            {
                var dto = SchoolRoleDto.MapFromSchoolRole(schoolRole);

                schoolRoleDtos.Add(dto);
            }

            return Ok(schoolRoleDtos);
        }

        [HttpGet("School_Roles/{id}")]
        public async Task<ActionResult<SchoolRoleDto>> GetSchoolRoleById(int id)
        {
            var schoolRole = await _unitOfWork.UserRepository
                .GetSchoolRoleByIdAsync(id);

            if (schoolRole == null)
            {
                return NotFound();
            }

            var schoolRoleDto = SchoolRoleDto.MapFromSchoolRole(schoolRole);

            return Ok(schoolRoleDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Classes/{id}")]
        public async Task<ActionResult<StudentsClassWithStudentsDto?>> GetStudentsClassById(int id)
        {
            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(id);

            if (studentsClass == null)
            {
                return NotFound(nameof(id));
            }

            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(studentsClass.SchoolId);

            if (school == null)
            {
                return NotFound(nameof(studentsClass.SchoolId));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(user));
            }

            var isUserInSchool = user.IsUserInSchool(school);

            if (!isUserInSchool)
            {
                return Unauthorized(
                    "Только пользователь, привязанный к школе, может взаимодействовать с ее классами");
            }

            var studentsClassDto = StudentsClassWithStudentsDto
                .MapFromStudentsClass(studentsClass);

            return Ok(studentsClassDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Classes")]
        public async Task<ActionResult<StudentsClassDto>> CreateStudentsClass(
            CreateStudentsClassDto createStudentsClassDto)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(createStudentsClassDto.SchoolId);

            if (school == null)
            {
                return NotFound(nameof(createStudentsClassDto.SchoolId));
            }

            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (clientId == null)
            {
                return BadRequest(nameof(clientId));
            }

            var intClientId = int.Parse(clientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(intClientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientUser));
            }

            var isUserAdminInSchool = clientUser.IsAdminInSchool(school);

            _logger.LogInformation($"isUserAdminInSchool: {isUserAdminInSchool}");

            if (!isUserAdminInSchool)
            {
                return Unauthorized(
                    "Только администрация может создавать классы в школе");
            }

            var studentsClass = createStudentsClassDto.MapToStudentsClass();

            _unitOfWork.UserRepository.AddClass(studentsClass);

            await _unitOfWork.SaveChangesAsync();

            foreach (var userId in createStudentsClassDto.StudentsIds)
            {
                var student = await _unitOfWork.UserRepository
                    .GetUserByIdAsync(userId);

                if (student == null)
                {
                    return NotFound(student);
                }

                await _unitOfWork.UserRepository
                    .AddUserToClassAsync(student, studentsClass, school);
            }

            await _unitOfWork.SaveChangesAsync();

            var createdClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(studentsClass.Id);

            if (createdClass == null)
            {
                return NotFound(nameof(createdClass));
            }

            var createdClassDto = StudentsClassDto
                .MapFromStudentsClass(createdClass);

            return CreatedAtAction(nameof(GetStudentsClassById),
                new
                {
                    createdClass.Id,
                },
                createdClassDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Classes/of_school/{schoolId}")]
        public async Task<ActionResult<IEnumerable<StudentsClassDto>>> ListSchoolClasses(
            int schoolId, string? query, int pageNum = 1, int pageSize = 10)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(schoolId));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return NotFound(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(user));
            }

            var isUserInSchool = user.IsUserInSchool(school);

            if (!isUserInSchool)
            {
                return Unauthorized(
                    "Только пользователь, привязанный к школе, может взаимодействовать с ее классами");
            }

            var classes = await _unitOfWork.UserRepository
                .ListClassesOfSchool(school, query, pageSize, pageNum);

            var classDtos = new List<StudentsClassDto>();

            foreach (var studentsClass in classes)
            {
                var dto = StudentsClassDto.MapFromStudentsClass(studentsClass);

                classDtos.Add(dto);
            }

            return Ok(classDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Classes/{id}")]
        public async Task<ActionResult> RemoveStudentsClass(int id)
        {
            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(id);

            if (studentsClass == null)
            {
                return NotFound(nameof(id));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            var isUserAdminInSchool = user.IsAdminInSchool(studentsClass.School);

            if (!isUserAdminInSchool)
            {
                return Unauthorized("Только администрация школы может удалять классы");
            }

            _unitOfWork.UserRepository.RemoveClass(studentsClass);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("Classes/{id}")]
        public async Task<ActionResult> UpdateStudentsClass(
            int id, JsonPatchDocument patchDocument)
        {
            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(id);

            if (studentsClass == null)
            {
                return NotFound(nameof(id));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            var isUserAdminInSchool = user.IsAdminInSchool(studentsClass.School);

            if (!isUserAdminInSchool)
            {
                return Unauthorized("Только администрация школы может изменять классы");
            }

            patchDocument.ApplyTo(studentsClass);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Classes/{classId}/add_user/{userId}")]
        public async Task<ActionResult> AddUserToStudentsClass(
            int classId, int userId)
        {
            var stringClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringClientId == null)
            {
                return BadRequest(nameof(stringClientId));
            }

            var clientId = int.Parse(stringClientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(clientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientId));
            }

            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(classId);

            if (studentsClass == null)
            {
                return NotFound(nameof(classId));
            }

            var isClientUserAdmin = clientUser.IsAdminInSchool(
                studentsClass.School);

            if (!isClientUserAdmin)
            {
                return Unauthorized(
                    "Только администрация школы может добавлять пользователей в класс");
            }

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            await _unitOfWork.UserRepository.AddUserToClassAsync(
                user, studentsClass, null);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Classes/{classId}/remove_user/{userId}")]
        public async Task<ActionResult> RemoveUserFromStudentsClassAsync(
            int classId, int userId)
        {
            var stringClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringClientId == null)
            {
                return BadRequest(nameof(stringClientId));
            }

            var clientId = int.Parse(stringClientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(clientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientId));
            }

            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(classId);

            if (studentsClass == null)
            {
                return NotFound(nameof(classId));
            }

            var isClientUserAdmin = clientUser
                .IsAdminInSchool(studentsClass.School);

            if (!isClientUserAdmin)
            {
                return Unauthorized(
                    "Только администрация школы может удалять пользователей из класса");
            }

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            await _unitOfWork.UserRepository.RemoveUserFromClassAsync(
                user, studentsClass);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{userId}/add_to_school/{schoolId}/with_role/{schoolRoleId}")]
        public async Task<ActionResult> AddUserWithRoleToSchool(
            int userId, int schoolId, int schoolRoleId)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(schoolId));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var intClientId = int.Parse(stringUserId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(intClientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientUser));
            }

            var isUserAdmin = await _userManager
                .IsInRoleAsync(clientUser, "admin");

            var isUserAdminInSchool = clientUser.IsAdminInSchool(school);

            _logger.LogInformation($"isUserAdmin: {isUserAdmin}; isUserAdminInSchool: {isUserAdminInSchool}");

            if (!(isUserAdminInSchool || isUserAdmin))
            {
                return Unauthorized(
                    "Только администрация школы или платформы может добавлять роли пользователям в школе");
            }

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            var schoolRole = await _unitOfWork.UserRepository
                .GetSchoolRoleByIdAsync(schoolRoleId);

            if (schoolRole == null)
            {
                return NotFound(nameof(schoolRoleId));
            }

            _unitOfWork.UserRepository
                .AddUserWithRoleToSchool(user, school, schoolRole);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{userId}/remove_role/{schoolRoleId}/from_school/{schoolId}")]
        public async Task<ActionResult> RemoveUsersRoleFromSchool(
            int userId, int schoolId, int schoolRoleId)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(schoolId));
            }

            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (clientId == null)
            {
                return BadRequest(nameof(clientId));
            }

            var intClientId = int.Parse(clientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(intClientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientUser));
            }

            var isUserAdmin = await _userManager
                .IsInRoleAsync(clientUser, "admin");

            var isUserAdminInSchool = clientUser.IsAdminInSchool(school);

            _logger.LogInformation($"isUserAdmin: {isUserAdmin}; isUserAdminInSchool: {isUserAdminInSchool}");

            if (!(isUserAdminInSchool || isUserAdmin))
            {
                return Unauthorized(
                    "Только администрация школы или платформы может убирать роли пользователей в школе");
            }

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            var schoolRole = await _unitOfWork.UserRepository
                .GetSchoolRoleByIdAsync(schoolRoleId);

            if (schoolRole == null)
            {
                return NotFound(nameof(schoolRoleId));
            }

            await _unitOfWork.UserRepository
                .RemoveRoleFromUserInSchoolAsync(user, school, schoolRole);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Role_Requests/send_to_school/{schoolId}/on_role/{roleId}")]
        public async Task<ActionResult> SendSchoolRoleRequest(int schoolId, int roleId)
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(user));
            }

            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(school));
            }

            var schoolRole = await _unitOfWork.UserRepository
                .GetSchoolRoleByIdAsync(roleId);

            if (schoolRole == null)
            {
                return NotFound(nameof(roleId));
            }

            var addRoleRequest = new AddRoleRequest()
            {
                UserId = userId,
                RoleId = roleId,
                SchoolId = schoolId
            };

            _unitOfWork.UserRepository.AddAddRoleRequest(addRoleRequest);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Role_Requests/of_school/{schoolId}")]
        public async Task<ActionResult<IEnumerable<AddRoleRequestAnnotationDto>>> ListRoleRequestOfSchool(
            int schoolId, int pageNum = 1, int pageSize = 10)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(schoolId));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                return BadRequest(nameof(stringUserId));
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(user));
            }

            var isUserAdminInSchool = user.IsAdminInSchool(school);

            if (!isUserAdminInSchool)
            {
                return Unauthorized(
                    "Только администрация школы может просматривать запросы пользователей на добавление с ролью");
            }

            var requests = await _unitOfWork.UserRepository
                .ListAddRoleRequestsOfSchool(school, pageNum, pageSize);

            var requestDtos = new List<AddRoleRequestAnnotationDto>();

            foreach (var request in requests)
            {
                var dto = AddRoleRequestAnnotationDto.MapFromAddRoleRequest(request);

                requestDtos.Add(dto);
            }

            return Ok(requestDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Role_Requests/{roleRequestId}/accept")]
        public async Task<ActionResult> AcceptRoleRequest(int roleRequestId)
        {
            var roleRequest = await _unitOfWork.UserRepository
                .GetAddRoleRequestByIdAsync(roleRequestId);

            if (roleRequest == null)
            {
                return NotFound(nameof(roleRequestId));
            }

            var stringClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringClientId == null)
            {
                return BadRequest(nameof(stringClientId));
            }

            var clientId = int.Parse(stringClientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(clientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientUser));
            }

            var isUserAdminInSchool = clientUser.IsAdminInSchool(roleRequest.School);

            _logger.LogInformation($"isUserAdminInSchool: {isUserAdminInSchool}");

            if (!isUserAdminInSchool)
            {
                return Unauthorized(
                    "Только администрация может взаимодействовать с запросами пользователей на роли в школе");
            }

            _unitOfWork.UserRepository.AcceptAddRoleRequest(roleRequest);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Role_Requests/{roleRequestId}/deny")]
        public async Task<ActionResult> DenyRoleRequest(int roleRequestId)
        {
            var roleRequest = await _unitOfWork.UserRepository
                .GetAddRoleRequestByIdAsync(roleRequestId);

            if (roleRequest == null)
            {
                return NotFound(nameof(roleRequestId));
            }

            var stringClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringClientId == null)
            {
                return BadRequest(nameof(stringClientId));
            }

            var clientId = int.Parse(stringClientId);

            var clientUser = await _unitOfWork.UserRepository
                .GetUserByIdAsync(clientId);

            if (clientUser == null)
            {
                return NotFound(nameof(clientUser));
            }

            var isUserAdminInSchool = clientUser.IsAdminInSchool(roleRequest.School);

            _logger.LogInformation($"isUserAdminInSchool: {isUserAdminInSchool}");

            if (!isUserAdminInSchool)
            {
                return Unauthorized(
                    "Только администрация может взаимодействовать с запросами пользователей на роли в школе");
            }

            _unitOfWork.UserRepository.DenyAddRoleRequest(roleRequest);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}

// TODO: Добавить просмотр ролей и ролей в школе в Get user by ID
// TODO: Вынести классы, роли в отдельные контроллеры