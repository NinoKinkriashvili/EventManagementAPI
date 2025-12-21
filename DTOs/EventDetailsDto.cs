namespace Pied_Piper.DTOs
{
    public class EventDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
        public int EventTypeId { get; set; }
    }
}