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

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return CreatedAtAction(nameof(RegisterUser), new
                {
                    user.Id

                }, user);
        }
    }
}
