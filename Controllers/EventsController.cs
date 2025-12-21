using Microsoft.AspNetCore.Mvc;
using Pied_Piper.Models;
using Pied_Piper.Repositories;
using Pied_Piper.DTOs;

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

            var result = events.Select(e => new EventListItemDto
            {
                Id = e.Id,
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                EndDateTime = e.EndDateTime,
                Location = e.Location,
                Capacity = e.Capacity
            });

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateEventDto dto)
        {
            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                EventTypeId = dto.EventTypeId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Location = dto.Location,
                Capacity = dto.Capacity,
                ImageUrl = dto.ImageUrl,
                CreatedById = dto.CreatedById
            };

            var created = await _eventRepository.CreateAsync(ev);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _eventRepository.GetByIdAsync(id);

            if (ev == null)
                return NotFound();

            var dto = new EventDetailsDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                Location = ev.Location,
                Capacity = ev.Capacity,
                ImageUrl = ev.ImageUrl,
                EventTypeId = ev.EventTypeId
            };

            return Ok(dto);
        }
    }
}