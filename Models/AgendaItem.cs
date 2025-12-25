namespace Pied_Piper.Models
{
    public class AgendaItem
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Time { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Property
        public Event Event { get; set; } = null!;
    }
}