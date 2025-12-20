namespace Pied_Piper.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime? CancelledAt { get; set; }

        // Navigation Properties
        public Event Event { get; set; } = null!;
        public User User { get; set; } = null!;
        public RegistrationStatus Status { get; set; } = null!;
    }
}
