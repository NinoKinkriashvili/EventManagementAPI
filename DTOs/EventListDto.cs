namespace Pied_Piper.DTOs;

public class EventListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EventTypeName { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public int ConfirmedCount { get; set; }
    public bool IsFull { get; set; }

    public string? ImageUrl { get; set; }
    public List<string> Tags { get; set; } = new();
}