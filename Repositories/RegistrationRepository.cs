using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.Models;

namespace Pied_Piper.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Registration?> GetByUserAndEventAsync(int userId, int eventId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId && r.Status.Name != "Cancelled");
        }

        public async Task<IEnumerable<Registration>> GetByUserIdAsync(int userId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Status)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Status)
                .Where(r => r.EventId == eventId)
                .OrderBy(r => r.RegisteredAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetAllAsync()
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.User)
                .Include(r => r.Status)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();
        }

        public async Task<Registration> CreateAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            // Load relations
            await _context.Entry(registration).Reference(r => r.Event).LoadAsync();
            await _context.Entry(registration).Reference(r => r.User).LoadAsync();
            await _context.Entry(registration).Reference(r => r.Status).LoadAsync();

            return registration;
        }

        public async Task<Registration> UpdateAsync(Registration registration)
        {
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();
            return registration;
        }

        public async Task DeleteAsync(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
        }
    }
}