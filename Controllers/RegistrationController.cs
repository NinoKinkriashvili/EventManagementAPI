using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.Repositories;
using Pied_Piper.DTOs;
using Pied_Piper.Models;
using System.Security.Claims;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ApplicationDbContext _context;

        public RegistrationController(
            IRegistrationRepository registrationRepository,
            IEventRepository eventRepository,
            ApplicationDbContext context)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
            _context = context;
        }

        // POST: api/registration/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterForEvent([FromBody] CreateRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get current user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            // Check if event exists
            var ev = await _eventRepository.GetByIdWithDetailsAsync(request.EventId);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            // Check if event is visible
            if (!ev.IsVisible)
                return BadRequest(new { message = "This event is not available for registration" });

            // Check if registration deadline has passed
            if (ev.RegistrationDeadline.HasValue && DateTime.UtcNow > ev.RegistrationDeadline)
                return BadRequest(new { message = "Registration deadline has passed" });

            // Check if event has already started
            if (DateTime.UtcNow > ev.EndDateTime)
                return BadRequest(new { message = "Event has already ended" });

            // Check if user is already registered
            var existingRegistration = await _registrationRepository.GetByUserAndEventAsync(userId, request.EventId);
            if (existingRegistration != null)
                return BadRequest(new { message = "You are already registered for this event" });

            // Get confirmed registrations count
            var confirmedCount = ev.Registrations.Count(r => r.Status.Name == "Confirmed");

            // Determine status: Confirmed or Waitlisted
            var statusName = "Confirmed";
            if (confirmedCount >= ev.MaxCapacity)
            {
                if (!ev.WaitlistEnabled)
                    return BadRequest(new { message = "Event is full and waitlist is not enabled" });

                // Check waitlist capacity
                var waitlistedCount = ev.Registrations.Count(r => r.Status.Name == "Waitlisted");
                if (ev.WaitlistCapacity.HasValue && waitlistedCount >= ev.WaitlistCapacity.Value)
                    return BadRequest(new { message = "Event is full and waitlist is also full" });

                statusName = "Waitlisted";
            }

            // Get status ID
            var status = await _context.RegistrationStatuses
                .FirstOrDefaultAsync(s => s.Name == statusName);

            if (status == null)
                return StatusCode(500, new { message = "Registration status not found" });

            // Create registration
            var registration = new Registration
            {
                EventId = request.EventId,
                UserId = userId,
                StatusId = status.Id,
                RegisteredAt = DateTime.UtcNow
            };

            var created = await _registrationRepository.CreateAsync(registration);

            return Ok(new
            {
                message = statusName == "Confirmed"
                    ? "Successfully registered for the event"
                    : "Event is full. You have been added to the waitlist",
                registrationId = created.Id,
                status = statusName,
                registration = new RegistrationDto
                {
                    Id = created.Id,
                    EventId = created.EventId,
                    EventTitle = created.Event.Title,
                    UserId = created.UserId,
                    UserFullName = created.User.FullName,
                    StatusName = created.Status.Name,
                    RegisteredAt = created.RegisteredAt
                }
            });
        }

        // DELETE: api/registration/unregister/{eventId}
        [HttpDelete("unregister/{eventId}")]
        public async Task<IActionResult> UnregisterFromEvent(int eventId)
        {
            // Get current user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            // Check if event exists
            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            // Check if event has already started
            if (DateTime.UtcNow > ev.StartDateTime)
                return BadRequest(new { message = "Cannot unregister from an event that has already started" });

            // Find user's registration
            var registration = await _registrationRepository.GetByUserAndEventAsync(userId, eventId);
            if (registration == null)
                return NotFound(new { message = "You are not registered for this event" });

            // Check if already cancelled
            if (registration.Status.Name == "Cancelled")
                return BadRequest(new { message = "Registration is already cancelled" });

            // Get cancelled status
            var cancelledStatus = await _context.RegistrationStatuses
                .FirstOrDefaultAsync(s => s.Name == "Cancelled");

            if (cancelledStatus == null)
                return StatusCode(500, new { message = "Cancelled status not found" });

            // Update registration status to cancelled
            registration.StatusId = cancelledStatus.Id;
            await _registrationRepository.UpdateAsync(registration);

            // If auto-approve is enabled and this was a confirmed registration, 
            // promote first waitlisted person
            if (ev.AutoApprove && registration.Status.Name == "Confirmed")
            {
                var firstWaitlisted = await _context.Registrations
                    .Include(r => r.Status)
                    .Where(r => r.EventId == eventId && r.Status.Name == "Waitlisted")
                    .OrderBy(r => r.RegisteredAt)
                    .FirstOrDefaultAsync();

                if (firstWaitlisted != null)
                {
                    var confirmedStatus = await _context.RegistrationStatuses
                        .FirstOrDefaultAsync(s => s.Name == "Confirmed");

                    if (confirmedStatus != null)
                    {
                        firstWaitlisted.StatusId = confirmedStatus.Id;
                        await _registrationRepository.UpdateAsync(firstWaitlisted);
                    }
                }
            }

            return Ok(new { message = "Successfully unregistered from the event" });
        }

        // GET: api/registration/my-registrations
        [HttpGet("my-registrations")]
        public async Task<IActionResult> GetMyRegistrations()
        {
            // Get current user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var registrations = await _registrationRepository.GetByUserIdAsync(userId);

            var result = registrations.Select(r => new RegistrationDto
            {
                Id = r.Id,
                EventId = r.EventId,
                EventTitle = r.Event.Title,
                UserId = r.UserId,
                UserFullName = r.User.FullName,
                StatusName = r.Status.Name,
                RegisteredAt = r.RegisteredAt
            });

            return Ok(result);
        }

        // GET: api/registration/event/{eventId}
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventRegistrations(int eventId)
        {
            // Check if event exists
            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            var registrations = await _registrationRepository.GetByEventIdAsync(eventId);

            var result = registrations
                .Where(r => r.Status.Name != "Cancelled") // Don't show cancelled
                .Select(r => new RegistrationDto
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = r.Event.Title,
                    UserId = r.UserId,
                    UserFullName = r.User.FullName,
                    StatusName = r.Status.Name,
                    RegisteredAt = r.RegisteredAt
                });

            return Ok(result);
        }

        // GET: api/registration/check/{eventId}
        [HttpGet("check/{eventId}")]
        public async Task<IActionResult> CheckRegistrationStatus(int eventId)
        {
            // Get current user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var registration = await _registrationRepository.GetByUserAndEventAsync(userId, eventId);

            if (registration == null)
            {
                return Ok(new { isRegistered = false, status = (string?)null });
            }

            return Ok(new
            {
                isRegistered = true,
                status = registration.Status.Name,
                registrationId = registration.Id,
                registeredAt = registration.RegisteredAt
            });
        }
    }
}