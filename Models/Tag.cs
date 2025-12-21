namespace Pied_Piper.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }

        // Navigation Properties
        public ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
    }
}
