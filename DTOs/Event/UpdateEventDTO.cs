namespace Pied_Piper.DTOs.Event
{
    public class UpdateEventDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EventTypeId { get; set; }
        public int CategoryId { get; set; } // NEW
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty; // NEW
        public int Capacity { get; set; }
        public int MinCapacity { get; set; } // NEW
        public int MaxCapacity { get; set; } // NEW
        public string? ImageUrl { get; set; }
        public List<int> TagIds { get; set; } = new();
    }
}