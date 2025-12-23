namespace Pied_Piper.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
