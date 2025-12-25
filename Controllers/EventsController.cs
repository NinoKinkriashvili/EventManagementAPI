using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pied_Piper.DTOs;
using Pied_Piper.DTOs.Event;
using Pied_Piper.Models;
using Pied_Piper.Repositories;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // Helper methods
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

        private static int GetAvailableSlots(Event e)
        {
            var confirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed");
            var available = e.MaxCapacity - confirmedCount;
            return available > 0 ? available : 0;
        }

        private static EventDetailsDto MapToDto(Event e)
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
                CurrentCapacity = confirmedCount,
                MaxCapacity = e.MaxCapacity,
                AvailableSlots = GetAvailableSlots(e),
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
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetAllAsync();
            var result = events.Select(e => MapToDto(e));
            return Ok(result);
        }

        // GET: api/events/sorted-by-popularity?n=5
        [HttpGet("sorted-by-popularity")]
        public async Task<IActionResult> GetAllSortedByPopularity([FromQuery] int? n)
        {
            var events = await _eventRepository.GetAllAsync();

            var query = events
                .Where(e => e.EndDateTime > DateTime.Now)
                .Select(e => new
                {
                    Event = e,
                    ConfirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed")
                })
                .OrderByDescending(x => x.ConfirmedCount)
                .Select(x => MapToDto(x.Event));

            var result = n.HasValue ? query.Take(n.Value).ToList() : query.ToList();

            return Ok(result);
        }

        // GET: api/events/sorted-by-upcoming-date?n=10
        [HttpGet("sorted-by-upcoming-date")]
        public async Task<IActionResult> GetAllSortedByUpcomingDate([FromQuery] int? n)
        {
            var events = await _eventRepository.GetAllAsync();

            var query = events
                .Where(e => e.EndDateTime > DateTime.Now)
                .OrderBy(e => e.StartDateTime)
                .Select(e => MapToDto(e));

            var result = n.HasValue ? query.Take(n.Value).ToList() : query.ToList();

            return Ok(result);
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);

            if (ev == null)
                return NotFound();

            var dto = MapToDto(ev);

            return Ok(dto);
        }

        // GET: api/events/types
        [HttpGet("types")]
        public async Task<IActionResult> GetEventTypes()
        {
            var types = await _eventRepository.GetEventTypesAsync();

            var result = types.Select(t => new EventTypeDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            });

            return Ok(result);
        }

        // GET: api/events/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _eventRepository.GetCategoriesAsync();

            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Title = c.Title
            });

            return Ok(result);
        }

        // GET: api/events/categories/search
        [HttpGet("categories/search")]
        public async Task<IActionResult> GetCategoryByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Category title is required");

            var category = await _eventRepository.GetCategoryByTitleAsync(title);

            if (category == null)
                return NotFound($"Category with title '{title}' not found");

            return Ok(new CategoryDto
            {
                Id = category.Id,
                Title = category.Title
            });
        }

        // GET: api/events/types/search
        [HttpGet("types/search")]
        public async Task<IActionResult> GetEventTypeByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Event type name is required");

            var eventType = await _eventRepository.GetEventTypeByNameAsync(name);

            if (eventType == null)
                return NotFound($"Event type with name '{name}' not found");

            return Ok(new EventTypeDto
            {
                Id = eventType.Id,
                Name = eventType.Name,
                Description = eventType.Description
            });
        }
    }
}