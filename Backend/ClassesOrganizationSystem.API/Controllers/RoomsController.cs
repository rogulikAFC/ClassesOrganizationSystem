using ClassesOrganizationSystem.Application.Models.RoomDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities.RoomEntities;
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
    public class RoomsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomsController> _logger;
        private readonly UserManager<User> _userManager;

        public RoomsController(
            IUnitOfWork unitOfWork, ILogger<RoomsController> logger,
            UserManager<User> user)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = user;
        }

        [HttpGet("in_school/{schoolId}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> ListRoomsInSchool(
            int schoolId, string? query, bool? isOpened, int pageSize = 10, int pageNum = 1)
        {
            var school = await _unitOfWork.SchoolRepository
                .GetSchoolByIdAsync(schoolId);

            if (school == null)
            {
                return NotFound(nameof(schoolId));
            }

            var rooms = await _unitOfWork.RoomRepository
                .ListSchoolRooms(school, isOpened, query, pageNum, pageSize);

            var roomDtos = new List<RoomDto>();

            foreach (var room in rooms)
            {
                roomDtos.Add(RoomDto.MapFromRoom(room));
            }

            return Ok(roomDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id)
        {
            var room = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound(nameof(id));
            }

            var roomDto = RoomDto.MapFromRoom(room);

            return Ok(roomDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto createRoomDto)
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
                .GetSchoolByIdAsync(createRoomDto.SchoolId);

            if (school == null)
            {
                return NotFound(nameof(createRoomDto.SchoolId));
            }

            if (!user.IsAdminInSchool(school))
            {
                return Unauthorized("Только администрация может добавлять кабинеты в школу");
            }

            var room = createRoomDto.MapToRoom();

            _unitOfWork.RoomRepository.AddRoom(room);

            var roomStatus = new RoomStatus
            {
                IsOpened = true,
            };

            _unitOfWork.RoomRepository.AddRoomStatus(roomStatus);

            _unitOfWork.RoomRepository.SetRoomStatusForRoom(room, roomStatus);

            await _unitOfWork.SaveChangesAsync();

            var createdRoom = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(room.Id);

            if (createdRoom == null)
            {
                return NotFound(nameof(createdRoom));
            }

            var roomDto = RoomDto.MapFromRoom(createdRoom);

            return CreatedAtAction(nameof(GetRoomById),
                new
                {
                    room.Id,
                },
                roomDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteRoom(int id)
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

            var room = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound(nameof(id));
            }

            if (!user.IsAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может удалять кабинеты из школы");
            }

            _unitOfWork.RoomRepository.RemoveRoom(room);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateRoom(
            int id, JsonPatchDocument patchDocument)
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

            var room = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound(nameof(id));
            }

            if (!user.IsAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может изменять кабинеты школы");
            }

            patchDocument.ApplyTo(room);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/Status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SetStatusToRoom(int id, AddRoomStatusDto addRoomStatusDto)
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

            var room = await _unitOfWork.RoomRepository
                .GetRoomByIdAsync(id);

            if (room == null)
            {
                return NotFound(nameof(id));
            }

            if (!user.IsAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может ставить кабинетам школы статус");
            }

            var roomStatus = addRoomStatusDto.MapToRoomStatus();

            _unitOfWork.RoomRepository.AddRoomStatus(roomStatus);

            _unitOfWork.RoomRepository.SetRoomStatusForRoom(room, roomStatus);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
