using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PolicyManagement.Services;
using PolicyManagement.Entities;
using Microsoft.AspNetCore.Authorization;
using PolicyManagement.DTOs;
namespace PolicyManagementApp.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            this.userService = _userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            return Ok(user);
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required" });
            }

            var createdUser = await userService.CreateUserAsync(user);
            return Ok(createdUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required" });   
            }
            var updatedUser = await userService.UpdateUserAsync(id, user);
            if (updatedUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            return Ok(updatedUser);
    }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var isDeleted = await userService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            return Ok("User deleted successfully");
        }
    }
}