using ClassesOrganizationSystem.Application.Models.LessonDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Aggregates;
using ClassesOrganizationSystem.Domain.Entities;
using ClassesOrganizationSystem.Domain.Entities.ScheduleEntites;
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
    public class LessonsSchedulesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LessonsSchedulesController> _logger;
        private readonly UserManager<User> _userManager;

        public LessonsSchedulesController(
            IUnitOfWork unitOfWork, ILogger<LessonsSchedulesController> logger,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<LessonsScheduleDto>> GetLessonsSchedule(int id)
        {
            var lessonsSchedule = await _unitOfWork.ScheduleRepository
                .GetLessonsScheduleByIdAsync(id);

            if (lessonsSchedule == null)
            {
                return NotFound(nameof(id));
            }

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

            if (!user.IsUserInSchool(lessonsSchedule.StudentsClass.School) || !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized(
                    "Только пользователь, привязанный к школе, может смотреть расписания занятий.");
            }

            var lessonsScheduleDto = LessonsScheduleDto
                .MapFromLessonsSchedule(lessonsSchedule);

            return Ok(lessonsScheduleDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<LessonsScheduleDto>> CreateLessonsSchedule(
            CreateLessonsScheduleDto createLessonsScheduleDto)
        {
            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(createLessonsScheduleDto.StudentsClassId);

            if (studentsClass == null)
            {
                return NotFound(nameof(studentsClass));
            }

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

            if (!user.IsAdminInSchool(studentsClass.School) || !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация школы может создавать расписания.");
            }

            var lessonsSchedule = createLessonsScheduleDto.MapToLessonsSchedule();

            _unitOfWork.ScheduleRepository.AddLessonsSchedule(lessonsSchedule);

            await _unitOfWork.SaveChangesAsync();

            var createdLessonsSchedule = await _unitOfWork.ScheduleRepository
                .GetLessonsScheduleByIdAsync(lessonsSchedule.Id);

            if (createdLessonsSchedule == null)
            {
                return NotFound(nameof(createdLessonsSchedule));
            }

            var lessonsScheduleDto = LessonsScheduleDto.MapFromLessonsSchedule(
                createdLessonsSchedule);

            return CreatedAtAction(nameof(GetLessonsSchedule), 
                new
                {
                    lessonsSchedule.Id,
                },
                lessonsScheduleDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> RemoveLessonsSchedule(int id)
        {
            var lessonsSchedule = await _unitOfWork.ScheduleRepository
                .GetLessonsScheduleByIdAsync(id);

            if (lessonsSchedule == null)
            {
                return NotFound(nameof(id));
            }

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

            if (!user.IsAdminInSchool(lessonsSchedule.StudentsClass.School) || !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация школы может удалять расписание.");
            }

            _unitOfWork.ScheduleRepository
                .RemoveLessonsSchedule(lessonsSchedule);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("for_class/{classId}/for_day")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<LessonsScheduleDto?>> GetLessonsScheduleForDayForClass(
            int classId, DateOnly? date, int? dayOfWeek)
        {
            if (!IsDateAndDayOfWeekValid(date, dayOfWeek))
            {
                return BadRequest("Дата и/или день недели указан(ы) неверно.");
            }

            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(classId);

            if (studentsClass == null)
            {
                return NotFound(nameof(classId));
            }

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

            if (!user.IsUserInSchool(studentsClass.School) && !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только пользователь, привязанный к школе, может просматривать расписание.");
            }

            LessonsSchedule? lessonsSchedule = null;

            if (date != null)
            {
                lessonsSchedule = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleByClassIdAndDateAsync(studentsClass, date.Value);
            }
            else if (dayOfWeek != null)
            {
                lessonsSchedule = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleByClassIdAndDayOfWeekAsync(studentsClass, dayOfWeek.Value);
            }

            if (lessonsSchedule == null)
            {
                return NotFound(nameof(lessonsSchedule));
            }

            var lessonsScheduleDto = LessonsScheduleDto
                .MapFromLessonsSchedule(lessonsSchedule);

            return lessonsScheduleDto;
        }

        [HttpGet("for_teacher/{teacherId}/for_day")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ListOfLessonsDto>> GetLessonsScheduleForDayForTeacher(
            int teacherId, DateOnly? date, int? dayOfWeek)
        {
            if (!IsDateAndDayOfWeekValid(date, dayOfWeek))
            {
                return BadRequest("Дата и/или день недели указан(ы) неверно.");
            }

            var teacher = await _unitOfWork.UserRepository
                .GetUserByIdAsync(teacherId);

            if (teacher == null)
            {
                return NotFound(nameof(teacherId));
            }

            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (stringUserId == null)
            {
                throw new Exception("Cannot find user id");
            }

            var userId = int.Parse(stringUserId);

            var user = await _unitOfWork.UserRepository
                .GetUserByIdAsync(userId);

            _logger.LogWarning("User ID and teacher ID: " + (userId == teacherId).ToString());
            _logger.LogWarning("User ID: " + userId + "   Teacher ID" + teacherId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            if (!((userId == teacherId) || await _userManager.IsInRoleAsync(user, "admin")))
            {
                return Unauthorized("Только учитель может просматривать расписания занятий для себя");
            }

            ListOfLessons listOfLessons = new ListOfLessons();

            if (date != null)
            {
                listOfLessons = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleForTeacherForDateAsync(teacher, date.Value);
            }
            else if (dayOfWeek != null)
            {
                listOfLessons = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleForTeacherByDayOfWeekAsync(teacher, dayOfWeek.Value);
            }

            var listOfLessonsDto = ListOfLessonsDto
                .MapFromListOfLessons(listOfLessons);

            return Ok(listOfLessonsDto);
        }

        [HttpGet("for_room/{roomId}/for_day")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ListOfLessonsDto>> GetLessonsScheduleForDayForRoom(
            int roomId, DateOnly? date, int? dayOfWeek)
        {
            if (!IsDateAndDayOfWeekValid(date, dayOfWeek))
            {
                return BadRequest("Дата и/или день недели указан(ы) неверно.");
            }

            var room = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(roomId);

            if (room == null)
            {
                return NotFound(nameof(roomId));
            }

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

            if (!(user.IsUserInSchool(room.School) || await _userManager.IsInRoleAsync(user, "admin")))
            {
                return Unauthorized("Только пользователь, привязанный к школе, может присматривать расписание.");
            }

            ListOfLessons listOfLessons = new ListOfLessons();

            if (date != null)
            {
                listOfLessons = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleForRoomForDateAsync(room, date.Value);
            }
            else if (dayOfWeek != null)
            {
                listOfLessons = await _unitOfWork.ScheduleRepository
                    .GetLessonsScheduleForRoomForDayOfWeekAsync(room, dayOfWeek.Value);
            }

            var listOfLessonsDto = ListOfLessonsDto
                .MapFromListOfLessons(listOfLessons);

            return Ok(listOfLessonsDto);
        }

        [HttpGet("for_class/{classId}/for_week")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<LessonsScheduleDto>>> GetLessonsSchedulesForClassForWeek(
            int classId, DateOnly? firstWeekDay)
        {
            var studentsClass = await _unitOfWork.UserRepository
                .GetClassByIdAsync(classId);

            if (studentsClass == null)
            {
                return NotFound(nameof(classId));
            }

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

            if (!(user.IsUserInSchool(studentsClass.School) || await _userManager.IsInRoleAsync(user, "admin")))
            {
                return Unauthorized("Только пользователь, привязанный к школе, может просматривать расписания.");
            }

            IEnumerable<LessonsSchedule> lessonsSchedules = new List<LessonsSchedule>();

            if (firstWeekDay != null)
            {
                lessonsSchedules = await _unitOfWork.ScheduleRepository
                    .GetLessonsSchedulesForWeekForClassAsync(studentsClass, firstWeekDay.Value);
            } 
            else if (firstWeekDay == null)
            {
                lessonsSchedules = await _unitOfWork.ScheduleRepository
                    .GetLessonsSchedulesForWeekForClassAsync(studentsClass);
            }

            var lessonsScheduleDtos = new List<LessonsScheduleDto>();

            foreach (var lessonsSchedule in lessonsSchedules)
            {
                var dto = LessonsScheduleDto
                    .MapFromLessonsSchedule(lessonsSchedule);

                lessonsScheduleDtos.Add(dto);
            }

            return Ok(lessonsScheduleDtos);
        }

        [HttpGet("for_teacher/{teacherId}/for_week/starts_with/{firstWeekDay}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<LessonsScheduleDto>>> GetLessonsSchedulesForTeacherForWeek(
            int teacherId, DateOnly firstWeekDay)
        {
            var teacher = await _unitOfWork.UserRepository
                .GetUserByIdAsync(teacherId);

            if (teacher == null)
            {
                return NotFound(nameof(teacherId));
            }

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

            if ((userId != teacherId) || !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только учитель может просматривать расписания занятий для себя");
            }

            var listsOfLessons = await _unitOfWork.ScheduleRepository
                .GetLessonsSchedulesForTeacherForWeekAsync(teacher, firstWeekDay);

            var listOfLessonDtos = new List<ListOfLessonsDto>();

            foreach (var listOfLessons in listsOfLessons)
            {
                var dto = ListOfLessonsDto
                    .MapFromListOfLessons(listOfLessons);

                listOfLessonDtos.Add(dto);
            }

            return Ok(listOfLessonDtos);
        }

        [HttpGet("for_teacher/{teacherId}/for_week")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<ListOfLessonsDto>>> GetLessonsSchedulesForTeacherForWeek(
            int teacherId)
        {
            var teacher = await _unitOfWork.UserRepository
                .GetUserByIdAsync(teacherId);

            if (teacher == null)
            {
                return NotFound(nameof(teacherId));
            }

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

            if ((userId != teacherId) || !await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только учитель может просматривать расписания занятий для себя");
            }

            var listsOfLessons = await _unitOfWork.ScheduleRepository
                .GetLessonsSchedulesForTeacherForWeekAsync(teacher);

            var listOfLessonDtos = new List<ListOfLessonsDto>();

            foreach (var listOfLessons in listsOfLessons)
            {
                var dto = ListOfLessonsDto
                    .MapFromListOfLessons(listOfLessons);

                listOfLessonDtos.Add(dto);
            }

            return Ok(listOfLessonDtos);
        }


        private bool IsDateAndDayOfWeekValid(
            DateOnly? date, int? dayOfWeek)
        {
            if (
                (dayOfWeek == null && date == null) 
                || (dayOfWeek != null && date != null)) // XOR
            {
                _logger.LogWarning("day of week and date are together");

                return false;
            }

            if (dayOfWeek != null && !(dayOfWeek >= 0 && dayOfWeek <= 6))
            {
                _logger.LogWarning("day of week is not in range from 0 to 6");

                return false;
            }

            _logger.LogInformation("date is valid");

            return true;
        }
    }
}
