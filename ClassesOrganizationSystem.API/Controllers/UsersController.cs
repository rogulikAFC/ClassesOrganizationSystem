using ClassesOrganizationSystem.Application.Models.UserDtos;
using ClassesOrganizationSystem.Application.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace ClassesOrganizationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUnitOfWork unitOfWork, ILogger<UsersController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
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
    }
}
