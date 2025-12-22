using Pied_Piper.Models;

namespace Pied_Piper.Repositories;

public interface IRegistrationRepository
{
    Task<Registration?> GetByIdAsync(int id);
    Task<IEnumerable<Registration>> GetByEventIdAsync(int eventId);
    Task<IEnumerable<Registration>> GetByUserIdAsync(int userId);

    Task<Registration> RegisterAsync(int eventId, int userId);
    Task CancelAsync(int registrationId);

    Task<int> GetConfirmedCountAsync(int eventId);
    Task<int> GetWaitlistedCountAsync(int eventId);
}