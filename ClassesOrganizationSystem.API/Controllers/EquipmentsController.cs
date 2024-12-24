using ClassesOrganizationSystem.Application.Models.RoomDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using ClassesOrganizationSystem.Domain.Entities.UserEntites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoomsController> _logger;
        private readonly UserManager<User> _userManager;

        public EquipmentsController(
            IUnitOfWork unitOfWork, ILogger<RoomsController> logger,
            UserManager<User> user)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = user;
        }

        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpDelete("{id}")]
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

        [HttpPatch("{id}")]
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

        [HttpPost("{equipmentId}/add_to_room/{roomId}")]
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

            if (!user.IsAdminInSchool(room.School))
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

        [HttpDelete("{equipmentId}/remove_from_room/{roomId}")]
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

            if (!user.IsAdminInSchool(room.School))
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
