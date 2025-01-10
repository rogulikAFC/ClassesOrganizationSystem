using ClassesOrganizationSystem.Application.Models.LessonDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using ClassesOrganizationSystem.Infrastructure.Persistence;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubjectsController> _logger;
        private readonly UserManager<User> _userManager;

        public SubjectsController(
            IUnitOfWork unitOfWork, ILogger<SubjectsController> logger,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDto>> GetSubjectById(int id)
        {
            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            var subjectDto = SubjectDto.MapFromSubject(subject);

            return Ok(subjectDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> ListSubjects(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            var subjects = await _unitOfWork.ScheduleRepository
                .ListSubjectsAsync(query, pageNum, pageSize);

            var subjectDtos = new List<SubjectDto>();

            foreach (var subject in subjects)
            {
                var subjectDto = SubjectDto.MapFromSubject(subject);

                subjectDtos.Add(subjectDto);
            }

            return Ok(subjectDtos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<SubjectDto>> CreateSubject(
            CreateSubjectDto createSubjectDto)
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

            var isUserAdmin = await _userManager.IsInRoleAsync(
                user, "admin");

            if (!isUserAdmin)
            {
                return Unauthorized("Только администрация может добавлять новые предметы.");
            }

            var subject = createSubjectDto.MapToSubject();

            _unitOfWork.ScheduleRepository.AddSubject(subject);

            await _unitOfWork.SaveChangesAsync();

            var subjectDto = SubjectDto.MapFromSubject(subject);

            return CreatedAtAction(nameof(GetSubjectById),
                new
                {
                    subject.Id
                },
                subjectDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateSubject(
            int id, [FromBody] JsonPatchDocument patchDocument)
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

            var isUserAdmin = await _userManager.IsInRoleAsync(
                user, "admin");

            if (!isUserAdmin)
            {
                return Unauthorized("Только администрация может обновлять предметы.");
            }

            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(subject);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveSubject(int id)
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

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

            var isUserAdmin = await _userManager.IsInRoleAsync(
                user, "admin");

            if (!isUserAdmin)
            {
                return Unauthorized("Только администрация может удалять предметы.");
            }

            var subject = await _unitOfWork.ScheduleRepository
                .GetSubjectByIdAsync(id);

            if (subject == null)
            {
                return NotFound(nameof(id));
            }

            _unitOfWork.ScheduleRepository
                .RemoveSubject(subject);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
