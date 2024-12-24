using ClassesOrganizationSystem.Application.Models.StudentsClassDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
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
    public class StudentsClassesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public StudentsClassesController(
            IUnitOfWork unitOfWork, ILogger<UsersController> logger,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpGet("of_school/{schoolId}")]
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
        [HttpDelete("{id}")]
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
        [HttpPatch("{id}")]
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
        [HttpPost("{classId}/add_user/{userId}")]
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
        [HttpDelete("{classId}/remove_user/{userId}")]
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
    }
}
