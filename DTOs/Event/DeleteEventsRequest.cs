using System.ComponentModel.DataAnnotations;

namespace Pied_Piper.DTOs.Event
{
    public class DeleteEventsRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one event ID is required")]
        public List<int> EventIds { get; set; } = new();
    }
}