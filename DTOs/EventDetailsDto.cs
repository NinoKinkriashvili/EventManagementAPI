namespace Pied_Piper.DTOs
{
    public class EventDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string EventTypeName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string Location { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;

        // Calculated field - current registrations
        public int CurrentCapacity { get; set; } // Current confirmed registrations
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }

        public bool WaitlistEnabled { get; set; }
        public int? WaitlistCapacity { get; set; }
        public bool AutoApprove { get; set; }

        public string? ImageUrl { get; set; }
        public bool IsVisible { get; set; }
        public int ConfirmedCount { get; set; }
        public int WaitlistedCount { get; set; }
        public bool IsFull { get; set; } // ConfirmedCount >= MaxCapacity
        public List<string> Tags { get; set; } = new();
        public string CreatedByName { get; set; } = string.Empty;
        public List<SpeakerDto> Speakers { get; set; } = new();
        public List<AgendaItemDto> Agenda { get; set; } = new();
    }
}