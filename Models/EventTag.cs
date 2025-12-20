namespace Pied_Piper.Models
{
    public class EventTag
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TagId { get; set; }

        // Navigation Properties
        public Event Event { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
