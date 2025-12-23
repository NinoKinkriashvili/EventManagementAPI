namespace Pied_Piper.DTOs.Event;

public class EventListDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string EventTypeName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryTitle { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public string VenueName { get; set; } = string.Empty;

    // Calculated
    public int CurrentCapacity { get; set; } // Current confirmed count
    public int MinCapacity { get; set; }
    public int MaxCapacity { get; set; }

    public int ConfirmedCount { get; set; }
    public bool IsFull { get; set; } // ConfirmedCount >= MaxCapacity
    public string? ImageUrl { get; set; }
    public List<string> Tags { get; set; } = new();
}