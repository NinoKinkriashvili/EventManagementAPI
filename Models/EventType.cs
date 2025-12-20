namespace Pied_Piper.Models
{
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
