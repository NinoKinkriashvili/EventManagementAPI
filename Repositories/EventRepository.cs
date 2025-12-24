using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.Models;

namespace Pied_Piper.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Include(e => e.CreatedBy)
                .Include(e => e.Speakers)
                .Include(e => e.AgendaItems)
                .Where(e => e.IsActive && e.IsVisible)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<Event?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Include(e => e.CreatedBy)
                .Include(e => e.Speakers)
                .Include(e => e.AgendaItems)
                .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<Event> CreateAsync(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ev = await GetByIdAsync(id);
            if (ev == null) return;

            ev.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Event>> GetUpcomingAsync()
        {
            return await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Where(e => e.IsActive && e.StartDateTime > DateTime.UtcNow)
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventType>> GetEventTypesAsync()
        {
            return await _context.EventTypes
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Events)
                .OrderBy(c => c.Title)
                .ToListAsync();
        }

        public async Task<EventType?> GetEventTypeByNameAsync(string name)
        {
            return await _context.EventTypes
                .FirstOrDefaultAsync(et => et.Name.ToLower() == name.ToLower());
        }

        public async Task<Category?> GetCategoryByTitleAsync(string title)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Title.ToLower() == title.ToLower());
        }

        public async Task<IEnumerable<Event>> GetAllIncludingHiddenAsync()
        {
            return await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Status)
                .Include(e => e.EventTags)
                    .ThenInclude(et => et.Tag)
                .Include(e => e.CreatedBy)
                .Include(e => e.Speakers)
                .Include(e => e.AgendaItems)
                .Where(e => e.IsActive)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
    }
}