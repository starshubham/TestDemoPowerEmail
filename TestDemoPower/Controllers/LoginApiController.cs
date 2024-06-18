using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDemoPower.ApplicationDbContext;
using TestDemoPower.Model;

namespace TestDemoPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {

        private readonly AppDbContext _context;
        public LoginApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Bad Request" });
            }

            // Check hardcoded credentials first
            if (request.Email == "admin@example.com" && request.Password == "admin123")
            {
                return Ok(new { message = "Admin logged in successfully with hardcoded credentials" });
            }

            // Fetch user based on email
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.EmailId == request.Email);

            if (user == null || user.Password != request.Password)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // If user  is found and password matches
            switch (user.Role.Name.ToLower())
            {
                case "admin":
                    return Ok(new { message = "Admin logged in successfully", roleId = user.Role.Id , UserId= user.Id });
                case "user":
                    return Ok(new { message = "User logged in successfully", roleId = user.Role.Id, UserId = user.Id });
                case "hr":
                    return Ok(new { message = "HR logged in successfully", roleId = user.Role.Id, UserId = user.Id });
                case "bd":
                    return Ok(new { message = "BD logged in successfully" , roleId = user.Role.Id, UserId = user.Id });
                default:
                    return Ok(new { message = "User logged in successfully" , roleId = user.Role.Id, UserId = user.Id });
            }
        }

    }
}
