using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.DTOs;
using Pied_Piper.DTOs.User;
using Pied_Piper.Models;
using Pied_Piper.Services;
using System.Security.Claims;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(
            ApplicationDbContext context,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
                return BadRequest(new { message = "Email is already registered" });

            // Get default department (e.g., "General" or first department)
            var defaultDepartment = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == "General" && d.IsActive);

            if (defaultDepartment == null)
            {
                // If no "General" department, get any active department
                defaultDepartment = await _context.Departments
                    .FirstOrDefaultAsync(d => d.IsActive);
            }

            if (defaultDepartment == null)
                return StatusCode(500, new { message = "No active departments found in database" });

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var user = new User
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = passwordHash,
                DepartmentId = defaultDepartment.Id,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load department for token generation
            await _context.Entry(user).Reference(u => u.Department).LoadAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user, user.Department.Name);
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "1440");

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Department = user.Department.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find user by email
            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // Check if user is active
            if (!user.IsActive)
                return Unauthorized(new { message = "Account is deactivated" });

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Invalid email or password" });

            // Generate JWT token
            var token = _jwtService.GenerateToken(user, user.Department.Name);
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "1440");

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Department = user.Department.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        // GET: api/auth/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            // Get user ID from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Department = user.Department.Name,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });
        }
    }
}