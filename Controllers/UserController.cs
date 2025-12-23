using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pied_Piper.Repositories;
using Pied_Piper.DTOs;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                DepartmentName = u.Department.Name, // Fixed: was u.Department.Name
                IsAdmin = u.IsAdmin,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });

            return Ok(result);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var dto = new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DepartmentName = user.Department.Name, // Fixed: was u.Department.Name
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                TotalRegistrations = user.Registrations.Count,
                TotalEventsCreated = user.CreatedEvents.Count
            };

            return Ok(dto);
        }

        // GET: api/user/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return NotFound(new { message = "User not found" });

            var dto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DepartmentName = user.Department.Name,
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Ok(dto);
        }
    }
}