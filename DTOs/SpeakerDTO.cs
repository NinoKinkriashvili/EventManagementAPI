namespace Pied_Piper.DTOs
{
    public class SpeakerDto
    {
        public int? Id { get; set; } // Nullable for create requests
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}