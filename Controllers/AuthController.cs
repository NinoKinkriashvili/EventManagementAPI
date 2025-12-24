using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.DTOs;
using Pied_Piper.DTOs.User;
using Pied_Piper.Models;
using Pied_Piper.Repositories;
using Pied_Piper.Services;
using System.Security.Claims;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public AuthController(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IJwtService jwtService,
            IConfiguration configuration,
            INotificationService notificationService)
        {
            _context = context;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
                return BadRequest(new { message = "Email is already registered" });

            var department = await _context.Departments.FindAsync(request.DepartmentId);
            if (department == null)
                return BadRequest(new { message = "Invalid department selected" });

            if (!department.IsActive)
                return BadRequest(new { message = "Selected department is not active" });

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                FullName = $"{request.FirstName} {request.LastName}",
                Password = passwordHash,
                PhoneNumber = request.PhoneNumber, 
                DepartmentId = request.DepartmentId,
                IsAdmin = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            await _notificationService.CreateWelcomeNotificationAsync(user.Id);

            return NoContent();
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            if (!user.IsActive)
                return Unauthorized(new { message = "Account is deactivated" });

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
                PhoneNumber = user.PhoneNumber,
                Department = user.Department.Name,
                IsAdmin = user.IsAdmin,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        // GET: api/auth/me
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Department = user.Department.Name,
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });
        }

        // POST: api/auth/refresh
        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null || !user.IsActive)
                return Unauthorized(new { message = "User not found or inactive" });

            // Generate new token
            var token = _jwtService.GenerateToken(user, user.Department.Name);
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "1440");

            return Ok(new LoginResponse
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Department = user.Department.Name,
                IsAdmin = user.IsAdmin,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        // GET: api/auth/departments
        [HttpGet("departments")]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    id = d.Id,
                    name = d.Name,
                    description = d.Description
                })
                .ToListAsync();

            return Ok(departments);
        }

        // POST: api/auth/check-otp
        [HttpPost("check-otp")]
        public IActionResult CheckOtp([FromBody] CheckOtpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool isValid = request.Otp == "111111";

            return Ok(isValid);
        }
    }
}