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

            var result = events.Select(e => new EventListDto
            {
                Id = e.Id,
                Title = e.Title,
                EventTypeName = e.EventType.Name,
                StartDateTime = e.StartDateTime,
                Location = e.Location,
                Capacity = e.Capacity,
                ImageUrl = e.ImageUrl,
                ConfirmedCount = e.Registrations.Count(r => r.Status.Name == "Confirmed"),
                IsFull = e.Registrations.Count(r => r.Status.Name == "Confirmed") >= e.Capacity,
                Tags = e.EventTags.Select(et => et.Tag.Name).ToList()
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
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                Location = ev.Location,
                Capacity = ev.Capacity,
                ImageUrl = ev.ImageUrl,
                ConfirmedCount = ev.Registrations.Count(r => r.Status.Name == "Confirmed"),
                WaitlistedCount = ev.Registrations.Count(r => r.Status.Name == "Waitlisted"),
                IsFull = ev.Registrations.Count(r => r.Status.Name == "Confirmed") >= ev.Capacity,
                Tags = ev.EventTags.Select(et => et.Tag.Name).ToList(),
                CreatedBy = ev.CreatedBy.FullName
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.StartDateTime >= request.EndDateTime)
                return BadRequest("StartDateTime must be before EndDateTime");

            var ev = new Event
            {
                Title = request.Title,
                Description = request.Description,
                EventTypeId = request.EventTypeId,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                Location = request.Location,
                Capacity = request.Capacity,
                ImageUrl = request.ImageUrl,
                CreatedById = 1  // დროებით
            };

            var created = await _eventRepository.CreateAsync(ev);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ev = await _eventRepository.GetByIdWithDetailsAsync(id);
            if (ev == null)
                return NotFound();

            var confirmedCount = ev.Registrations.Count(r => r.Status.Name == "Confirmed");
            if (request.Capacity < confirmedCount)
                return BadRequest("Capacity cannot be less than confirmed registrations");

            ev.Title = request.Title;
            ev.Description = request.Description;
            ev.EventTypeId = request.EventTypeId;
            ev.StartDateTime = request.StartDateTime;
            ev.EndDateTime = request.EndDateTime;
            ev.Location = request.Location;
            ev.Capacity = request.Capacity;
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
    }
}