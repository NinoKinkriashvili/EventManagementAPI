using Pied_Piper.DTOs.Event;
using System.ComponentModel.DataAnnotations;

namespace Pied_Piper.DTOs
{
    public class UpdateEventRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int EventTypeId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public DateTime? RegistrationDeadline { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        public int MinCapacity { get; set; }

        [Required]
        [Range(1, 10000)]
        public int MaxCapacity { get; set; }

        public bool WaitlistEnabled { get; set; } = true;
        public int? WaitlistCapacity { get; set; }
        public bool AutoApprove { get; set; } = true;

        public string? ImageUrl { get; set; }

        public List<int> TagIds { get; set; } = new();

        public List<SpeakerDto> Speakers { get; set; } = new();
        public List<AgendaItemDto> Agenda { get; set; } = new();
    }
}