using Pied_Piper.Data;
using Pied_Piper.Models;

namespace Pied_Piper.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateWelcomeNotificationAsync(int userId)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = "Welcome to Pied Piper Events!",
                Message = "Your account has been created successfully. Start exploring and registering for exciting events!",
                Type = "Welcome",
                IsSeen = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRegistrationNotificationAsync(int userId, int eventId, string eventTitle, string status)
        {
            var message = status == "Confirmed"
                ? $"You have successfully registered for '{eventTitle}'. Your spot is confirmed!"
                : $"You have been added to the waitlist for '{eventTitle}'. We'll notify you if a spot becomes available.";

            var notification = new Notification
            {
                UserId = userId,
                EventId = eventId,
                Title = status == "Confirmed" ? "Registration Confirmed" : "Added to Waitlist",
                Message = message,
                Type = "Registration",
                IsSeen = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateUnregistrationNotificationAsync(int userId, int eventId, string eventTitle)
        {
            var notification = new Notification
            {
                UserId = userId,
                EventId = eventId,
                Title = "Unregistration Confirmed",
                Message = $"You have been unregistered from '{eventTitle}'. We hope to see you at future events!",
                Type = "Unregistration",
                IsSeen = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task CreateEventUpdateNotificationAsync(int userId, int eventId, string eventTitle, string updateMessage)
        {
            var notification = new Notification
            {
                UserId = userId,
                EventId = eventId,
                Title = "Event Update",
                Message = $"Update for '{eventTitle}': {updateMessage}",
                Type = "EventUpdate",
                IsSeen = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}