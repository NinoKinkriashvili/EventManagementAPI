using Microsoft.AspNetCore.Mvc;
using Pied_Piper.Repositories;
using Pied_Piper.DTOs;
using Pied_Piper.Models;

namespace Pied_Piper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventRepository.GetAllAsync();

            var result = events.Select(e => new EventDetailsDto
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
                CurrentCapacity = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                MinCapacity = e.MinCapacity,
                MaxCapacity = e.MaxCapacity,
                WaitlistEnabled = e.WaitlistEnabled,
                WaitlistCapacity = e.WaitlistCapacity,
                AutoApprove = e.AutoApprove,
                ImageUrl = e.ImageUrl,
                IsVisible = e.IsVisible,
                ConfirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                WaitlistedCount = e.Registrations.Count(r => r.Status.Name == "Waitlisted"),
                IsFull = e.Registrations.Count(r => r.Status.Name == "Confirmed") >= e.MaxCapacity,
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
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);

            if (ev == null)
                return NotFound();

            var dto = new EventDetailsDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                EventTypeName = ev.EventType.Name,
                CategoryId = ev.CategoryId,
                CategoryTitle = ev.Category.Title,
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                RegistrationDeadline = ev.RegistrationDeadline,
                Location = ev.Location,
                VenueName = ev.VenueName,
                CurrentCapacity = ev.Registrations.Count(r => r.Status.Name == "Confirmed"), // CALCULATED
                MinCapacity = ev.MinCapacity,
                MaxCapacity = ev.MaxCapacity,
                WaitlistEnabled = ev.WaitlistEnabled,
                WaitlistCapacity = ev.WaitlistCapacity,
                AutoApprove = ev.AutoApprove,
                ImageUrl = ev.ImageUrl,
                IsVisible = ev.IsVisible,
                ConfirmedCount = ev.Registrations.Count(r => r.Status.Name == "Confirmed"),
                WaitlistedCount = ev.Registrations.Count(r => r.Status.Name == "Waitlisted"),
                IsFull = ev.Registrations.Count(r => r.Status.Name == "Confirmed") >= ev.MaxCapacity, // Use MaxCapacity
                Tags = ev.EventTags.Select(et => et.Tag.Name).ToList(),
                CreatedByName = ev.CreatedBy.FullName,
                Speakers = ev.Speakers.Select(s => new SpeakerDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Role = s.Role,
                    PhotoUrl = s.PhotoUrl
                }).ToList(),
                Agenda = ev.AgendaItems.Select(a => new AgendaItemDto
                {
                    Id = a.Id,
                    Time = a.Time,
                    Title = a.Title,
                    Description = a.Description
                }).ToList()
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
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
                // REMOVED: Capacity = request.Capacity,
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

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("many")]
        public async Task<IActionResult> DeleteMany([FromBody] List<int> eventIds)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);
            if (ev == null)
                return NotFound();

            // Validation
            if (request.StartDateTime >= request.EndDateTime)
                return BadRequest("StartDateTime must be before EndDateTime");

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
            // REMOVED: ev.Capacity = request.Capacity;
            ev.MinCapacity = request.MinCapacity;
            ev.MaxCapacity = request.MaxCapacity;
            ev.WaitlistEnabled = request.WaitlistEnabled;
            ev.WaitlistCapacity = request.WaitlistCapacity;
            ev.AutoApprove = request.AutoApprove;
            ev.ImageUrl = request.ImageUrl;

            await _eventRepository.UpdateAsync(ev);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);
            if (ev == null)
                return NotFound();

            await _eventRepository.DeleteAsync(id);

            return NoContent();
        }

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

        [HttpPatch("{id}/visibility")]
        public async Task<IActionResult> UpdateVisibility(int id, [FromBody] UpdateVisibilityRequest request)
        {
            var ev = await _eventRepository.GetByIdAsync(id);

            if (ev == null)
                return NotFound($"Event with ID {id} not found");

            ev.IsVisible = request.IsVisible;
            ev.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(ev);

            return Ok(new { message = $"Event visibility updated to {request.IsVisible}", isVisible = ev.IsVisible });
        }

        [HttpGet("all-including-hidden")]
        public async Task<IActionResult> GetAllIncludingHidden()
        {
            var events = await _eventRepository.GetAllIncludingHiddenAsync();

            var result = events.Select(e => new EventDetailsDto
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
                CurrentCapacity = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                MinCapacity = e.MinCapacity,
                MaxCapacity = e.MaxCapacity,
                WaitlistEnabled = e.WaitlistEnabled,
                WaitlistCapacity = e.WaitlistCapacity,
                AutoApprove = e.AutoApprove,
                ImageUrl = e.ImageUrl,
                IsVisible = e.IsVisible,
                ConfirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                WaitlistedCount = e.Registrations.Count(r => r.Status.Name == "Waitlisted"),
                IsFull = e.Registrations.Count(r => r.Status.Name == "Confirmed") >= e.MaxCapacity,
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
            });

            return Ok(result);
        }
    }
}