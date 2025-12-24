namespace Pied_Piper.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Registration", "Unregistration", "Welcome", "EventUpdate"
        public int? EventId { get; set; } // Nullable - only for event-related notifications
        public bool IsSeen { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;
        public Event? Event { get; set; }
    }
}