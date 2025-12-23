namespace Pied_Piper.Models
{
    public class AgendaItem
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Time { get; set; } = string.Empty; // e.g., "09:00 AM" or "09:00-10:00"
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Property
        public Event Event { get; set; } = null!;
    }
}