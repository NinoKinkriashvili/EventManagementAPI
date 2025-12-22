using System.ComponentModel.DataAnnotations;

namespace Pied_Piper.DTOs;

public class CreateEventRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public int EventTypeId { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    [Required]
    public string Location { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }

    public List<int> TagIds { get; set; } = new();
}