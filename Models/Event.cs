namespace Pied_Piper.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EventTypeId { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string Location { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public bool WaitlistEnabled { get; set; } = true;
        public int? WaitlistCapacity { get; set; } // null = unlimited
        public bool AutoApprove { get; set; } = true; // Auto-approve from waitlist

        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public EventType EventType { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
        public ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
        public ICollection<AgendaItem> AgendaItems { get; set; } = new List<AgendaItem>();
    }
}