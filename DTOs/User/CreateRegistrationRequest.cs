using System.ComponentModel.DataAnnotations;

namespace Pied_Piper.DTOs
{
    public class CreateRegistrationRequest
    {
        [Required]
        public int EventId { get; set; }
    }
}