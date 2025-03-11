using ClassesOrganizationSystem.Application.Models.UserDtos;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}