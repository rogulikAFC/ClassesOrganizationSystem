using ClassesOrganizationSystem.Application.Models.SchoolDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SchoolsController> _logger;

        public SchoolsController(
            IUnitOfWork unitOfWork, ILogger<SchoolsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolDto>> GetSchoolById(int id)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(id);

            if (school == null)
            {
                return NotFound(nameof(id));
            }

            var schoolDto = SchoolDto.MapFromSchool(school);

            return Ok(schoolDto);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<SchoolAnnotationDto>>> ListSchools(
            string? query, int pageNum = 1, int pageSize = 5)
        {
            var schools = await _unitOfWork.SchoolRepository
                .ListSchoolsAsync(query, pageNum, pageSize);

            var schoolDtos = new List<SchoolAnnotationDto>();

            foreach (var school in schools)
            {
                var schoolDto = SchoolAnnotationDto.MapFromSchool(school);

                schoolDtos.Add(schoolDto);
            }

            return Ok(schoolDtos);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SchoolDto>> CreateSchool(
            CreateSchoolDto createSchoolDto)
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

            _logger.LogInformation(user.ToString());

            var school = createSchoolDto.MapToSchool();

            await _unitOfWork.SchoolRepository.AddSchool(school, user);

            await _unitOfWork.SaveChangesAsync();

            var schoolDto = SchoolDto.MapFromSchool(school);

            return CreatedAtAction(nameof(GetSchoolById),
                new
                {
                    school.Id
                },
                schoolDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteSchool(int id)
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

            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(id);

            if (school == null)
            {
                return NotFound(nameof(id));
            }

            if (!user.IsUserAdminInSchool(school))
            {
                return Unauthorized("Только администрация может удалить школу");
            }

            _unitOfWork.SchoolRepository.RemoveSchool(school);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PatchSchool(int id, JsonPatchDocument patchDocument)
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

            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(id);

            if (school == null)
            {
                return NotFound(nameof(id));
            }

            if (!user.IsUserAdminInSchool(school))
            {
                return Unauthorized("Только администрация может изменять данные школы");
            }

            patchDocument.ApplyTo(school);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
