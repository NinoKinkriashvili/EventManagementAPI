namespace Pied_Piper.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }

        // Navigation Property
        public Event Event { get; set; } = null!;
    }
}