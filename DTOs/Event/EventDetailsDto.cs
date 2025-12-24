using Pied_Piper.DTOs.Event;

namespace Pied_Piper.DTOs
{
    public class EventDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string EventTypeName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public string Location { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;

        // Capacity info
        public int CurrentCapacity { get; set; } // NEW - How many are registered
        public int MaxCapacity { get; set; }
        public int AvailableSlots { get; set; } // How many spots left
        public string EventStatus { get; set; } = string.Empty; // "Available", "Waitlisted", "Full"

        public bool AutoApprove { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsVisible { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string CreatedByName { get; set; } = string.Empty;
        public List<SpeakerDto> Speakers { get; set; } = new List<SpeakerDto>();
        public List<AgendaItemDto> Agenda { get; set; } = new List<AgendaItemDto>();
    }
}