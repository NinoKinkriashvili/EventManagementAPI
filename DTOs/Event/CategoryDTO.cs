namespace Pied_Piper.DTOs.Event
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalEvents { get; set; }
    }
}