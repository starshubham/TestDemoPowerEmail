using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDemoPower.ApplicationDbContext;
using TestDemoPower.Model;
using TestDemoPower.Services;

namespace TestDemoPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                // Ensure the role ID is valid
                var role = await _context.Roles.FindAsync(user.RoleId);
                if (role == null)
                {
                    return BadRequest(new { message = "Invalid role ID" });
                }

                // Check if the email already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.EmailId == user.EmailId);
                if (existingUser != null)
                {
                    return Conflict(new { message = "Email already exists" });
                }

                // Assign role to user
                user.Role = role;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Send registration details via email
                try
                {
                    _emailService.SendRegistrationDetails(user.EmailId, user.Username, user.Password);
                }
                catch (Exception ex)
                {
                    // Handle email sending failure
                    return BadRequest(new { message = $"Failed to send registration details via email: {ex.Message}" });
                }

                // Remove the role object from the response
                user.Role = null;

                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(ModelState);
        }

    }
}
