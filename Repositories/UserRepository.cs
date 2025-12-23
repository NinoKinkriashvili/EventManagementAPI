using Microsoft.EntityFrameworkCore;
using Pied_Piper.Data;
using Pied_Piper.Models;

namespace Pied_Piper.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Department)
                .Include(u => u.Registrations)
                    .ThenInclude(r => r.Event)
                .Include(u => u.CreatedEvents)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Department)
                .Where(u => u.IsActive)
                .OrderBy(u => u.FullName)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Load the department after creation
            await _context.Entry(user).Reference(u => u.Department).LoadAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();
            }
        }
    }
}