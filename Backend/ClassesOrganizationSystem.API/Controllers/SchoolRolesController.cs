using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolRolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public SchoolRolesController(
            IUnitOfWork unitOfWork, ILogger<UsersController> logger,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
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
        [HttpPost("add_user/{userId}/to_school/{schoolId}/with_role/{schoolRoleId}")]
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
        [HttpDelete("remove_user/{userId}/from_school/{schoolId}/with_role/{schoolRoleId}")]
        public async Task<ActionResult> RemoveUserWithRoleFromSchool(
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
        [HttpPost("Requests/send_to_school/{schoolId}/on_role/{roleId}")]
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
        [HttpGet("Requests/of_school/{schoolId}")]
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
        [HttpPost("Requests/{roleRequestId}/accept")]
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
        [HttpDelete("Requests/{roleRequestId}/deny")]
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
