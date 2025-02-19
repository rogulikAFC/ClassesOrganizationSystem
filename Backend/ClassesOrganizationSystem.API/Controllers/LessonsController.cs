using ClassesOrganizationSystem.Application.Models.LessonDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LessonsController> _logger;

        public LessonsController(
            IUnitOfWork unitOfWork, UserManager<User> userManager
            , ILogger<LessonsController> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLessonById(int id)
        {
            var lesson = await _unitOfWork.ScheduleRepository
                .GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound(nameof(id));
            }

            var lessonDto = LessonDto.MapFromLesson(lesson);

            return Ok(lessonDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SubjectDto>> CreateLesson(
            CreateLessonDto createLessonDto)
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                throw new Exception("Cannot find user id");
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация может добавлять предметы.");
            }

            var lesson = createLessonDto.MapToLesson();

            _unitOfWork.ScheduleRepository.AddLesson(lesson);

            await _unitOfWork.SaveChangesAsync();

            var createdLesson = await _unitOfWork.ScheduleRepository
                .GetLessonByIdAsync(lesson.Id);

            if (createdLesson == null)
            {
                return NotFound(createdLesson);
            }

            var createdLessonDto = LessonDto.MapFromLesson(createdLesson);

            return CreatedAtAction(nameof(GetLessonById),
                new
                {
                    lesson.Id
                },
                createdLessonDto);
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateLesson(
            int id, JsonPatchDocument patchDocument)
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                throw new Exception("Cannot find user id");
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация может изменять предметы.");
            }

            var lesson = await _unitOfWork.ScheduleRepository
                .GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(lesson);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> RemoveLesson(int id)
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                throw new Exception("Cannot find user id");
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация может удалять предметы.");
            }

            var lesson = await _unitOfWork.ScheduleRepository
                .GetLessonByIdAsync(id);

            if (lesson == null)
            {
                return NotFound(nameof(id));
            }

            _unitOfWork.ScheduleRepository
                .RemoveLesson(lesson);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
