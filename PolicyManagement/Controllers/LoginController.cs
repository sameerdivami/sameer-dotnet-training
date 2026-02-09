using Microsoft.AspNetCore.Mvc;
using PolicyManagement.Services;

namespace PolicyManagementApp.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginService;

        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }

        public record LoginRequest(string Email, string Password);

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Console.WriteLine("Inside the Login method of LoginController");
            var token = await _loginService.AuthenticateAsync(
                request.Email,
                request.Password
            );

            if (token == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            return Ok(new
            {
                Message = "Login successful",
                Token = token
            });
        }
    }
}
