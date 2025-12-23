using Pied_Piper.Models;

namespace Pied_Piper.Repositories
{
    public interface IRegistrationRepository
    {
        Task<Registration?> GetByIdAsync(int id);
        Task<Registration?> GetByUserAndEventAsync(int userId, int eventId);
        Task<IEnumerable<Registration>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
        Task<IEnumerable<Registration>> GetAllAsync();
        Task<Registration> CreateAsync(Registration registration);
        Task<Registration> UpdateAsync(Registration registration);
        Task DeleteAsync(int id);
    }
}