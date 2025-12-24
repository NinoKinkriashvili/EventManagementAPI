namespace Pied_Piper.Services
{
    public interface INotificationService
    {
        Task CreateWelcomeNotificationAsync(int userId);
        Task CreateRegistrationNotificationAsync(int userId, int eventId, string eventTitle, string status);
        Task CreateUnregistrationNotificationAsync(int userId, int eventId, string eventTitle);
        Task CreateEventUpdateNotificationAsync(int userId, int eventId, string eventTitle, string updateMessage);
    }
}