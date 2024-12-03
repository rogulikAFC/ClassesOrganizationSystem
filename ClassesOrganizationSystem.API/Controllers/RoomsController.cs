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
using System.Text.Json;

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

        [HttpGet("Equipments")]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> ListEquipments(
            string? query, int pageNum = 1, int pageSize = 10)
        {
            var equipments = await _unitOfWork.RoomRepository
                .ListEquipments(query, pageNum, pageSize);

            var equipmentDtos = new List<EquipmentDto>();

            foreach (var equipment in equipments)
            {
                equipmentDtos.Add(EquipmentDto.MapFromEquipment(equipment));
            }

            return Ok(equipmentDtos);
        }

        [HttpGet("Equipments/{id}")]
        public async Task<ActionResult<EquipmentDto>> GetEquipmentById(int id)
        {
            var equipment = await _unitOfWork.RoomRepository
                .GetEquipmentByIdAsync(id);

            if (equipment == null)
            {
                return NotFound(id);
            }

            var equipmentDto = EquipmentDto.MapFromEquipment(equipment);

            return Ok(equipmentDto);
        }

        [HttpPost("Equipments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<EquipmentDto>> CreateEquipment(
            CreateEquipmentDto createEquipmentDto)
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

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация платформы может создавать оборудование");
            }

            var equipment = createEquipmentDto.MapToEquipment();

            _unitOfWork.RoomRepository.CreateEquipment(equipment);

            await _unitOfWork.SaveChangesAsync();

            var equipmentDto = EquipmentDto.MapFromEquipment(equipment);

            return CreatedAtAction(nameof(GetEquipmentById), 
                new
                {
                    equipment.Id
                },
                equipmentDto);
        }

        [HttpDelete("Equipments/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteEquipment(int id)
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

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация платформы может удалять оборудование");
            }

            var equipment = await _unitOfWork.RoomRepository
                .GetEquipmentByIdAsync(id);

            if (equipment == null)
            {
                return NotFound(id);
            }

            _unitOfWork.RoomRepository.RemoveEquipment(equipment);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("in_school/{schoolId}")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> ListRoomsInSchool(
            int schoolId, string? query, int pageSize = 10, int pageNum = 1, bool isOpened = false)
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

            if (!user.IsUserAdminInSchool(school))
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

            if (!user.IsUserAdminInSchool(room.School))
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

            if (!user.IsUserAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может изменять кабинеты школы");
            }

            patchDocument.ApplyTo(room);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("Equipments/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateEquipment(
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

            if (!await _userManager.IsInRoleAsync(user, "admin"))
            {
                return Unauthorized("Только администрация платформы может изменять данные об оборудовании");
            }

            var equipment = await _unitOfWork.RoomRepository
                .GetEquipmentByIdAsync(id);

            if (equipment == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(equipment);

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

            if (!user.IsUserAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может ставить кабинетам школы статус");
            }

            var roomStatus = addRoomStatusDto.MapToRoomStatus();

            _unitOfWork.RoomRepository.AddRoomStatus(roomStatus);

            _unitOfWork.RoomRepository.SetRoomStatusForRoom(room, roomStatus);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{roomId}/add_equipment/{equipmentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> AddEquipmentToRoom(int roomId, int equipmentId)
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
                .GetRoomByIdAsync(roomId);

            if (room == null)
            {
                return NotFound(nameof(roomId));
            }

            if (!user.IsUserAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может добавлять оборудование в кабинеты");
            }

            var equipment = await _unitOfWork.RoomRepository
                .GetEquipmentByIdAsync(equipmentId);

            if (equipment == null)
            {
                return NotFound(nameof(equipmentId));
            }

            _unitOfWork.RoomRepository
                .AddEquipmentToRoom(room, equipment);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{roomId}/remove_equipment/{equipmentId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> RemoveEquipmentFromRoom(int roomId, int equipmentId)
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
                .GetRoomByIdAsync(roomId);

            if (room == null)
            {
                return NotFound(nameof(roomId));
            }

            if (!user.IsUserAdminInSchool(room.School))
            {
                return Unauthorized("Только администрация может удалять оборудование из кабинета");
            }

            var equipment = await _unitOfWork.RoomRepository
                .GetEquipmentByIdAsync(equipmentId);

            if (equipment == null)
            {
                return NotFound(nameof(equipmentId));
            }

            await _unitOfWork.RoomRepository
                .RemoveEquipmentFromRoomAsync(room, equipment);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
