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

        public async Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId)
        {
            return await _context.Registrations
                .Where(r => r.EventId == eventId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Registration>> GetByUserIdAsync(int userId)
        {
            return await _context.Registrations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        // Register, Cancel დავამატოთ შემდეგ ეტაპზე
        public Task<Registration> RegisterAsync(int eventId, int userId)
            => throw new NotImplementedException();

        public Task CancelAsync(int registrationId)
            => throw new NotImplementedException();

        public Task<int> GetConfirmedCountAsync(int eventId)
            => throw new NotImplementedException();

        public Task<int> GetWaitlistedCountAsync(int eventId)
            => throw new NotImplementedException();
    }
}