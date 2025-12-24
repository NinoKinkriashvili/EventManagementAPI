using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.DTOs;
using Pied_Piper.DTOs.Event;
using Pied_Piper.Models;
using Pied_Piper.Repositories;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    // [Authorize] // Uncomment when you want to protect all admin endpoints
    public class AdminController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly ApplicationDbContext _context;

        public AdminController(IEventRepository eventRepository, ApplicationDbContext context)
        {
            _eventRepository = eventRepository;
            _context = context;
        }

        [HttpGet("events/all-including-hidden")]
        public async Task<IActionResult> GetAllIncludingHidden()
        {
            var events = await _eventRepository.GetAllIncludingHiddenAsync();

            var result = events.Select(e =>
            {
                var confirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed");

                return new EventDetailsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventTypeName = e.EventType.Name,
                    CategoryId = e.CategoryId,
                    CategoryTitle = e.Category.Title,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    RegistrationDeadline = e.RegistrationDeadline,
                    Location = e.Location,
                    VenueName = e.VenueName,
                    CurrentCapacity = confirmedCount, // NEW
                    MaxCapacity = e.MaxCapacity,
                    AvailableSlots = e.MaxCapacity - confirmedCount,
                    EventStatus = GetEventStatus(e),
                    AutoApprove = e.AutoApprove,
                    ImageUrl = e.ImageUrl,
                    IsVisible = e.IsVisible,
                    Tags = e.EventTags.Select(et => et.Tag.Name).ToList(),
                    CreatedByName = e.CreatedBy.FullName,
                    Speakers = e.Speakers.Select(s => new SpeakerDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Role = s.Role,
                        PhotoUrl = s.PhotoUrl
                    }).ToList(),
                    Agenda = e.AgendaItems.Select(a => new AgendaItemDto
                    {
                        Id = a.Id,
                        Time = a.Time,
                        Title = a.Title,
                        Description = a.Description
                    }).ToList()
                };
            });

            return Ok(result);
        }

        // Helper method for AdminController
        private static string GetEventStatus(Event e)
        {
            var confirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed");

            if (confirmedCount >= e.MaxCapacity)
            {
                if (e.WaitlistEnabled)
                {
                    return "Waitlisted";
                }
                return "Full";
            }

            return "Available";
        }

        // POST: api/admin/events
        [HttpPost("events")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validation
            if (request.StartDateTime >= request.EndDateTime)
                return BadRequest("StartDateTime must be before EndDateTime");

            if (request.MinCapacity > request.MaxCapacity)
                return BadRequest("MinCapacity cannot be greater than MaxCapacity");

            var ev = new Event
            {
                Title = request.Title,
                Description = request.Description,
                EventTypeId = request.EventTypeId,
                CategoryId = request.CategoryId,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                RegistrationDeadline = request.RegistrationDeadline,
                Location = request.Location,
                VenueName = request.VenueName,
                MinCapacity = request.MinCapacity,
                MaxCapacity = request.MaxCapacity,
                WaitlistEnabled = request.WaitlistEnabled,
                WaitlistCapacity = request.WaitlistCapacity,
                AutoApprove = request.AutoApprove,
                ImageUrl = request.ImageUrl,
                IsVisible = request.IsVisible,
                CreatedById = 1  // TODO: Get from authentication
            };

            var created = await _eventRepository.CreateAsync(ev);

            // Add speakers if provided
            if (request.Speakers != null && request.Speakers.Any())
            {
                foreach (var speakerDto in request.Speakers)
                {
                    var speaker = new Speaker
                    {
                        EventId = created.Id,
                        Name = speakerDto.Name,
                        Role = speakerDto.Role,
                        PhotoUrl = speakerDto.PhotoUrl
                    };
                    _context.Speakers.Add(speaker);
                }
                await _context.SaveChangesAsync();
            }

            // Add agenda items if provided
            if (request.Agenda != null && request.Agenda.Any())
            {
                foreach (var agendaDto in request.Agenda)
                {
                    var agendaItem = new AgendaItem
                    {
                        EventId = created.Id,
                        Time = agendaDto.Time,
                        Title = agendaDto.Title,
                        Description = agendaDto.Description
                    };
                    _context.AgendaItems.Add(agendaItem);
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Event created successfully", eventId = created.Id });
        }

        // PUT: api/admin/events/{id}
        [HttpPut("events/{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);
            if (ev == null)
                return NotFound();

            // Validation
            if (request.StartDateTime >= request.EndDateTime)
                return BadRequest("StartDateTime must be before EndDateTime");

            if (request.RegistrationDeadline.HasValue && request.RegistrationDeadline >= request.StartDateTime)
                return BadRequest("Registration deadline must be before event start time");

            if (request.MinCapacity > request.MaxCapacity)
                return BadRequest("MinCapacity cannot be greater than MaxCapacity");

            // Check if new MaxCapacity is less than current confirmed registrations
            var confirmedCount = ev.Registrations.Count(r => r.Status.Name == "Confirmed");
            if (request.MaxCapacity < confirmedCount)
                return BadRequest($"MaxCapacity cannot be less than confirmed registrations ({confirmedCount})");

            // Update properties
            ev.Title = request.Title;
            ev.Description = request.Description;
            ev.EventTypeId = request.EventTypeId;
            ev.CategoryId = request.CategoryId;
            ev.StartDateTime = request.StartDateTime;
            ev.EndDateTime = request.EndDateTime;
            ev.RegistrationDeadline = request.RegistrationDeadline;
            ev.Location = request.Location;
            ev.VenueName = request.VenueName;
            ev.MinCapacity = request.MinCapacity;
            ev.MaxCapacity = request.MaxCapacity;
            ev.WaitlistEnabled = request.WaitlistEnabled;
            ev.WaitlistCapacity = request.WaitlistCapacity;
            ev.AutoApprove = request.AutoApprove;
            ev.ImageUrl = request.ImageUrl;
            ev.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(ev);

            // Update speakers: Remove old ones and add new ones
            if (request.Speakers != null)
            {
                var existingSpeakers = _context.Speakers.Where(s => s.EventId == id);
                _context.Speakers.RemoveRange(existingSpeakers);

                foreach (var speakerDto in request.Speakers)
                {
                    var speaker = new Speaker
                    {
                        EventId = id,
                        Name = speakerDto.Name,
                        Role = speakerDto.Role,
                        PhotoUrl = speakerDto.PhotoUrl
                    };
                    _context.Speakers.Add(speaker);
                }
                await _context.SaveChangesAsync();
            }

            // Update agenda items: Remove old ones and add new ones
            if (request.Agenda != null)
            {
                var existingAgenda = _context.AgendaItems.Where(a => a.EventId == id);
                _context.AgendaItems.RemoveRange(existingAgenda);

                foreach (var agendaDto in request.Agenda)
                {
                    var agendaItem = new AgendaItem
                    {
                        EventId = id,
                        Time = agendaDto.Time,
                        Title = agendaDto.Title,
                        Description = agendaDto.Description
                    };
                    _context.AgendaItems.Add(agendaItem);
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Event updated successfully" });
        }

        // DELETE: api/admin/events/{id}
        [HttpDelete("events/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null)
                return NotFound();

            await _eventRepository.DeleteAsync(id);

            return Ok(new { message = "Event deleted successfully" });
        }

        // DELETE: api/admin/events/bulk
        [HttpDelete("events/bulk")]
        public async Task<IActionResult> DeleteManyEvents([FromBody] List<int> eventIds)
        {
            if (eventIds == null || !eventIds.Any())
                return BadRequest("No event IDs provided");

            var notFoundIds = new List<int>();
            var deletedIds = new List<int>();

            foreach (var id in eventIds)
            {
                var ev = await _eventRepository.GetByIdAsync(id);
                if (ev == null)
                {
                    notFoundIds.Add(id);
                    continue;
                }

                await _eventRepository.DeleteAsync(id);
                deletedIds.Add(id);
            }

            if (notFoundIds.Any())
            {
                return Ok(new
                {
                    message = $"Deleted {deletedIds.Count} events. {notFoundIds.Count} events not found.",
                    deletedIds,
                    notFoundIds
                });
            }

            return Ok(new
            {
                message = $"Successfully deleted {deletedIds.Count} events",
                deletedIds
            });
        }

        // PATCH: api/admin/events/{id}/visibility
        [HttpPatch("events/{id}/visibility")]
        public async Task<IActionResult> UpdateEventVisibility(int id, [FromBody] UpdateVisibilityRequest request)
        {
            var ev = await _eventRepository.GetByIdAsync(id);

            if (ev == null)
                return NotFound($"Event with ID {id} not found");

            ev.IsVisible = request.IsVisible;
            ev.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(ev);

            return Ok(new { message = $"Event visibility updated to {request.IsVisible}", isVisible = ev.IsVisible });
        }

        // GET: api/admin/events/{eventId}/waitlist
        [HttpGet("events/{eventId}/waitlist")]
        public async Task<IActionResult> GetEventWaitlist(int eventId)
        {
            var ev = await _eventRepository.GetByIdAsync(eventId);
            if (ev == null)
                return NotFound(new { message = "Event not found" });

            var waitlistedRegistrations = await _context.Registrations
                .Include(r => r.User)
                    .ThenInclude(u => u.Department)
                .Include(r => r.Status)
                .Where(r => r.EventId == eventId && r.Status.Name == "Waitlisted")
                .OrderBy(r => r.RegisteredAt)  // First in line comes first
                .ToListAsync();

            var result = waitlistedRegistrations.Select((r, index) => new
            {
                position = index + 1,  // Position in waitlist
                registrationId = r.Id,
                userId = r.UserId,
                userName = r.User.FullName,
                userEmail = r.User.Email,
                department = r.User.Department.Name,
                registeredAt = r.RegisteredAt
            });

            return Ok(new
            {
                eventId = eventId,
                eventTitle = ev.Title,
                totalWaitlisted = waitlistedRegistrations.Count,
                waitlistCapacity = ev.WaitlistCapacity,
                waitlist = result
            });
        }
    }
}