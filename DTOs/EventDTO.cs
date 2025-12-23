namespace Pied_Piper.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty; // NEW
        public int Capacity { get; set; }
        public int MinCapacity { get; set; } // NEW
        public int MaxCapacity { get; set; } // NEW
        public string? ImageUrl { get; set; }
        public string EventType { get; set; } = string.Empty; // "In-Person", "Virtual", "Hybrid"
        public int CategoryId { get; set; } // NEW
        public string CategoryTitle { get; set; } = string.Empty; // NEW - "Sports", "Workshop", etc.
    }
}